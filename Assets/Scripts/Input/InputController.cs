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

        void Awake()
        {
            basicMovement = GetComponent<BasicMovement>();
            if (basicMovement == null)
                Debug.LogError("Movement component not found!");
        }

        void Update()
        {
            // Apply movement if there is movement input
            float movementInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(movementInput) > 0)
                basicMovement.Move(BasicMovement.MoveDirection.Right, movementInput);

            // Apply jump if there is jump input
            if (Input.GetButtonDown("Jump"))
                basicMovement.Jump();
        }
    }
}
