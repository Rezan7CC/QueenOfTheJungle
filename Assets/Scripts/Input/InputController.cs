using UnityEngine;
using System.Collections;
using QJMovement;

namespace QJInput
{
    public class InputController : MonoBehaviour
    {
        /// <summary>
        /// Cached BasicMovement component
        /// </summary>
        BasicMovement basicMovement;

        /// <summary>
        /// Cached Hang component
        /// </summary>
        Hang hang;

        void Awake()
        {
            basicMovement = GetComponent<BasicMovement>();
            if (basicMovement == null)
                Debug.LogError("Movement component not found!");

            hang = GetComponent<Hang>();
        }

        void Update()
        {
            // Apply movement if there is movement input
            float movementInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(movementInput) > 0)
            {
                basicMovement.MovementState = BasicMovement.EMovementState.Walking;
                basicMovement.Move(BasicMovement.MoveDirection.Right, movementInput);
            }
            else
                basicMovement.MovementState = BasicMovement.EMovementState.Idle;

            // Apply jump if there is jump input
            if (Input.GetButtonDown("Jump"))
            {
                // Deactivate hanging if character is currently hanging instead of performing jump
                if (hang != null && hang.IsHanging)
                    hang.DeactiveHanging();
                else
                    basicMovement.Jump();
            }

            // Deactivate hanging on positive vertical movement
            if (Input.GetAxisRaw("Vertical") > 0 && hang != null && hang.IsHanging)
                hang.DeactiveHanging();

            // Apply drop if there is drop input
            if (Input.GetButtonDown("Drop") && hang != null)
                hang.ActivateHanging();
        }
    }
}
