using UnityEngine;
using System.Collections;

namespace QJMovement
{
    /// <summary>
    /// Should replace CharacterController2D. Currently acts as an adapter
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// Cached CharacterController2D component.
        /// Handles the movement of the character
        /// </summary>
        CharacterController2D characterController2D;

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

        void Awake()
        {
            characterController2D = GetComponent<CharacterController2D>();
            if (characterController2D == null)
                Debug.LogError("CharacterController2D not found!");
        }

        // Update is called once per frame
        void Update()
        {
            characterController2D.ignoreOneWayPlatformsThisFrame = IgnoreOneWayPlatforms;
        }

        void LateUpdate()
        {
            characterController2D.ignoreOneWayPlatformsThisFrame = IgnoreOneWayPlatforms;
        }

        public void Move(Vector2 deltaMovement)
        {
            if (characterController2D == null)
                return;

            characterController2D.move(new Vector3(deltaMovement.x, deltaMovement.y, 0));
        }
    }
}
