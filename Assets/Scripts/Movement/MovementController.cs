using UnityEngine;
using System.Collections;

namespace QJMovement
{
    /// <summary>
    /// Should replace CharacterController2D. Currently acts as an adapter
    /// </summary>
    [RequireComponent(typeof(CharacterController2D), typeof(BoxCollider2D))]
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// Cached CharacterController2D component.
        /// Handles the movement of the character
        /// </summary>
        CharacterController2D characterController2D;

        /// <summary>
        /// Cached BoxCollider2D component.
        /// </summary>
        BoxCollider2D boxCollider2D;

        Vector2 distanceBetweenRays = Vector2.zero;

        bool gravityEnabled = true;

        Vector2 velocity = Vector2.zero;

        public bool IsGrounded
        {
            get
            {
                return characterController2D != null
                    && characterController2D.isGrounded;
            }
            private set { }
        }

        public bool IgnoreOneWayPlatforms
        {
            get; set;
        }

        public bool GravityEnabled
        {
            get { return gravityEnabled; }
            set { gravityEnabled = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public LayerMask OneWayPlatformMask
        {
            get
            {
                if (characterController2D == null)
                    return new LayerMask();
                return characterController2D.oneWayPlatformMask;
            }
        }

        Vector2 DistanceBetweenRays
        {
            get
            {
                if (distanceBetweenRays != Vector2.zero)
                    return distanceBetweenRays;

                if (boxCollider2D == null || characterController2D != null)
                    return Vector2.zero;

                distanceBetweenRays.x = boxCollider2D.bounds.max.x / characterController2D.totalVerticalRays;
                distanceBetweenRays.y = boxCollider2D.bounds.max.y / characterController2D.totalHorizontalRays;
                return distanceBetweenRays;
            }
        }

        void Awake()
        {
            characterController2D = GetComponent<CharacterController2D>();
            if (characterController2D == null)
                Debug.LogError("CharacterController2D not found!");

            boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
                Debug.LogError("BoxCollider2D not found!");
        }

        // Update is called once per frame
        void Update()
        {
            characterController2D.ignoreOneWayPlatformsThisFrame = IgnoreOneWayPlatforms;
        }

        void LateUpdate()
        {
            characterController2D.ignoreOneWayPlatformsThisFrame = IgnoreOneWayPlatforms;

            // Apply gravity if character is not grounded
            if (!IsGrounded && gravityEnabled)
                velocity.y += Physics2D.gravity.y * Time.deltaTime;

            // Apply the velocity
            if (!Mathf.Approximately(velocity.x, 0) || !Mathf.Approximately(velocity.y, 0))
                Move(velocity * Time.deltaTime);
        }

        public void Move(Vector2 deltaMovement)
        {
            if (characterController2D == null)
                return;

            characterController2D.move(new Vector3(deltaMovement.x, deltaMovement.y, 0));
        }

        public Collider2D IsGroundedOn(LayerMask groundLayer)
        {
            if (!IsGrounded || boxCollider2D == null || characterController2D == null)
                return null;

            Vector2 initialRayOrigin = boxCollider2D.bounds.min;

            for (int i = 0; i < characterController2D.totalVerticalRays; i++)
            {
                Vector2 rayOrigin = new Vector2(initialRayOrigin.x + i * DistanceBetweenRays.x, initialRayOrigin.y);

                RaycastHit2D groundedRayHit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.01f, groundLayer);
                if (groundedRayHit && groundedRayHit.transform != null)
                    return groundedRayHit.collider;
            }
            return null;
        }
    }
}
