using UnityEngine;
using System.Collections;

namespace QJMovement
{
    [RequireComponent(typeof(MovementController))]
    public class BasicMovement : MonoBehaviour
    {
        /// <summary>
        /// Whether the owner can walk
        /// </summary>
        [SerializeField]
        bool canWalk = true;

        /// <summary>
        /// Whether the owner can sprint
        /// </summary>
        [SerializeField]
        bool canSprint = true;

        /// <summary>
        /// Whether the owner can jump
        /// </summary>
        [SerializeField]
        bool canJump = true;

        /// <summary>
        /// Whether the owner can double jump
        /// </summary>
        [SerializeField]
        bool canDoubleJump = true;

        /// <summary>
        /// Whether the owner can drop down from one directional platforms
        /// </summary>
        [SerializeField]
        bool canDropDown = true;

        /// <summary>
        /// The speed for walking
        /// </summary>
        [SerializeField]
        float walkingSpeed = 5;

        /// <summary>
        /// The speed for sprinting
        /// </summary>
        [SerializeField]
        float sprintingSpeed = 7.5f;

        /// <summary>
        /// The force for jumps
        /// </summary>
        [SerializeField]
        float jumpForce = 6;

        /// <summary>
        /// The force for double jumps
        /// </summary>
        [SerializeField]
        float doubleJumpForce = 4;

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
        /// Updates the position via MovementController and resets every frame
        /// </summary>
        Vector2 motion = Vector2.zero;

        /// <summary>
        /// Whether a double jump is active
        /// </summary>
        bool isDoubleJumpActive = false;

        /// <summary>
        /// Cached Movement controller component.
        /// Handles the movement of the character
        /// </summary>
        MovementController movementController;

        public enum MoveDirection
        {
            Left, Right
        }

        public enum EMovementState
        {
            Idle, Walking, Sprinting
        }

        /// <summary>
        /// Whether the owner can walk
        /// </summary>
        public bool CanWalk { get { return canWalk; } set { canWalk = value; } }

        /// <summary>
        /// Whether the owner can sprint
        /// </summary>
        public bool CanSprint { get { return canSprint; } set { canSprint = value; } }

        /// <summary>
        /// Whether the owner can jump
        /// </summary>
        public bool CanJump { get { return canJump; } set { canJump = value; } }

        /// <summary>
        /// Whether the owner can double jump
        /// </summary>
        public bool CanDoubleJump { get { return canJump; } set { canJump = value; } }

        /// <summary>
        /// Whether the owner can drop down from one directional platforms
        /// </summary>
        public bool CanDropDown { get { return canDropDown; } set { canDropDown = value; } }

        /// <summary>
        /// The speed for walking
        /// </summary>
        public float WalkingSpeed { get { return walkingSpeed; } set { walkingSpeed = value; } }

        /// <summary>
        /// The speed for sprinting
        /// </summary>
        public float SprintingSpeed { get { return sprintingSpeed; } set { sprintingSpeed = value; } }

        /// <summary>
        /// Current movement state
        /// </summary>
        public EMovementState MovementState { get; set; }

        /// <summary>
        /// The force for jumps
        /// </summary>
        public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }

        /// <summary>
        /// The force for double jumps
        /// </summary>
        public float DoubleJumpForce { get { return doubleJumpForce; } set { doubleJumpForce = value; } }

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
        /// Updates the position via MovementController and resets every frame
        /// </summary>
        Vector2 Motion { get { return motion; }}

        void Awake()
        {
            movementController = GetComponent<MovementController>();
            if (movementController == null)
                Debug.LogError("MovementController not found!");
        }

        void LateUpdate()
        {
            // Apply gravity if character is not grounded
            if (!movementController.IsGrounded)
                verticalVelocity += Physics2D.gravity.y * Time.deltaTime;
            else
                OnGrounded();

            // Apply the vertical velocity if there is one
            if (!Mathf.Approximately(verticalVelocity, 0))
                motion.y += verticalVelocity * Time.deltaTime;

            // Update the position with the current motion
            if (motion != Vector2.zero)
            {
                movementController.Move(motion);
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

            float movement = 0;

            if (canWalk && MovementState == EMovementState.Walking)
                movement = walkingSpeed;
            else if (canSprint && MovementState == EMovementState.Sprinting)
                movement = sprintingSpeed;

            if (movement == 0)
                return;

            if (moveDirection == MoveDirection.Left)
                movement *= -1;

            movement *= factor * Time.deltaTime;

            if (!movementController.IsGrounded)
                movement *= airControl;

            motion.x += movement;

            // Update rotation
            Vector3 localRotation = transform.localRotation.eulerAngles;

            if (movement < 0)
                localRotation.y = 180;
            else if (movement > 0)
                localRotation.y = 0;

            transform.localRotation = Quaternion.Euler(localRotation);
        }

        /// <summary>
        /// Starts a jump if character is grounded.
        /// Affects vertical velocity
        /// </summary>
        public void Jump()
        {
            if (canJump && movementController.IsGrounded)
                verticalVelocity = jumpForce;

            else if(canDoubleJump && !isDoubleJumpActive)
            {
                isDoubleJumpActive = true;
                verticalVelocity = doubleJumpForce;
            }
        }

        /// <summary>
        /// Drops down through one directional platforms
        /// </summary>
        public void Drop()
        {
            if (!canDropDown)
                return;

            StartCoroutine(DropThroughPlatform());
        }

        /// <summary>
        /// Disables character collision for one directional platforms for a short duration
        /// </summary>
        /// <returns></returns>
        IEnumerator DropThroughPlatform()
        {
            movementController.IgnoreOneWayPlatforms = true;

            float delay = 0.05f;
            while(delay > 0)
            {
                delay -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            movementController.IgnoreOneWayPlatforms = false;
        }

        /// <summary>
        /// Event that gets called on impact with ground
        /// </summary>
        void OnGrounded()
        {
            isDoubleJumpActive = false;
        }
    }
}