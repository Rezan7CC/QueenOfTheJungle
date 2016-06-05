using UnityEngine;
using System.Collections;

namespace QJMovement
{
    public class BasicMovement : MonoBehaviour
    {
        /// <summary>
        /// The speed for normal movement
        /// </summary>
        [SerializeField]
        float movementSpeed = 5;

        /// <summary>
        /// The force for jumps
        /// </summary>
        [SerializeField]
        float jumpForce = 6;

        /// <summary>
        /// The factor for movement speed while in the air
        /// </summary>
        [SerializeField]
        float airControl = 0.75f;

        /// <summary>
        /// The current vertical velocity.
        /// Affected by jumps and gravity and affects motion
        /// </summary>
        float verticalVelocity = 0;

        /// <summary>
        /// The current frame motion.
        /// Updates the position via CharacterController2D and resets every frame
        /// </summary>
        Vector2 motion = Vector2.zero;

        /// <summary>
        /// Cached CharacterController2D component.
        /// Handles the movement of the character
        /// </summary>
        CharacterController2D characterController2D;

        public enum MoveDirection
        {
            Left, Right
        }

        /// <summary>
        /// The speed for normal movement
        /// </summary>
        public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }

        /// <summary>
        /// The force for jumps
        /// </summary>
        public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }

        /// <summary>
        /// The factor for movement speed while in the air
        /// </summary>
        public float AirControl { get { return airControl; } set { airControl = value; } }

        /// <summary>
        /// The current vertical velocity.
        /// Affected by jumps and gravity and affects motion
        /// </summary>
        public float VerticalVelocity { get { return verticalVelocity; } set { verticalVelocity = value; } }

        /// <summary>
        /// The current frame motion.
        /// Updates the position via CharacterController2D and resets every frame
        /// </summary>
        Vector2 Motion { get { return motion; }}

        void Awake()
        {
            characterController2D = GetComponent<CharacterController2D>();
            if (characterController2D == null)
                Debug.LogError("CharacterController2D not found!");
        }

        void LateUpdate()
        {
            // Apply gravity if character is not grounded
            if (!characterController2D.isGrounded)
                verticalVelocity += Physics2D.gravity.y * Time.deltaTime;

            // Apply the vertical velocity if there is one
            if (!Mathf.Approximately(verticalVelocity, 0))
                motion.y += verticalVelocity * Time.deltaTime;

            // Update the position with the current motion
            if (motion != Vector2.zero)
            {
                characterController2D.move(motion);
                motion = Vector2.zero;
            }
        }

        /// <summary>
        /// Normal movement to left or right.
        /// Affected by Time.deltaTime and AirControl and affects motion
        /// </summary>
        /// <param name="moveDirection">Direction for the movement</param>
        /// <param name="factor">Factor for movement that is clamped between -1 and 1</param>
        public void Move(MoveDirection moveDirection, float factor)
        {
            factor = Mathf.Clamp(factor, -1.0f, 1.0f);
            float movement = (moveDirection == MoveDirection.Right ? movementSpeed : -movementSpeed) * factor * Time.deltaTime;
            if (!characterController2D.isGrounded)
                movement *= airControl;
            motion.x += movement;
        }

        /// <summary>
        /// Starts a jump if character is grounded.
        /// Affects vertical velocity
        /// </summary>
        public void Jump()
        {
            if (characterController2D.isGrounded)
                verticalVelocity = jumpForce;
        }
    }
}