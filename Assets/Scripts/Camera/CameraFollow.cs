using UnityEngine;
using System.Collections;

namespace QJCamera
{
    public class CameraFollow : MonoBehaviour
    {
        /// <summary>
        /// Target transform for the camera follow movement
        /// </summary>
        [SerializeField]
        Transform followTarget;

        /// <summary>
        /// Position offset for the camera follow target
        /// </summary>
        [SerializeField]
        Vector2 targetOffset = Vector2.zero;

        /// <summary>
        /// Duration until the camera reaches the camera follow target
        /// </summary>
        [SerializeField]
        float smoothDuration = 0.5f;

        /// <summary>
        /// Max camera follow movement speed
        /// </summary>
        [SerializeField]
        float maxSpeed = 20;

        /// <summary>
        /// Max height for the camera follow target
        /// </summary>
        [SerializeField]
        float maxHeight = 10;

        /// <summary>
        /// Min height for the camera follow target
        /// </summary>
        [SerializeField]
        float minHeight = 1;

        /// <summary>
        /// Current smoothed velocity for the camera movement
        /// </summary>
        Vector2 currentVelocity = Vector2.zero;

        /// <summary>
        /// Target transform for the camera follow movement
        /// </summary>
        public Transform FollowTarget { get { return followTarget; } set { followTarget = value; } }

        /// <summary>
        /// Position offset for the camera follow target
        /// </summary>
        public Vector2 TargetOffset { get { return targetOffset; } set { targetOffset = value; } }

        /// <summary>
        /// Duration until the camera reaches the camera follow target
        /// </summary>
        public float SmoothDuration { get { return smoothDuration; } set { smoothDuration = value; } }

        /// <summary>
        /// Max camera follow movement speed
        /// </summary>
        public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

        /// <summary>
        /// Max height for the camera follow target
        /// </summary>
        public float MaxHeight { get { return maxHeight; } set { maxHeight = value; } }

        /// <summary>
        /// Min height for the camera follow target
        /// </summary>
        public float MinHeight { get { return minHeight; } set { minHeight = value; } }

        void Start()
        {
            if (followTarget == null)
                Debug.LogError("Camera follow target is null!");
        }

        void Update()
        {
            if (followTarget == null)
                return;

            Vector3 cameraPosition = transform.position;
            Vector2 targetPosition = ((Vector2)followTarget.position) + targetOffset;
            targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);

            Vector2 tempCameraPosition =
            Vector2.SmoothDamp(cameraPosition, targetPosition,
                               ref currentVelocity, smoothDuration,
                               maxSpeed, Time.deltaTime);

            cameraPosition.x = tempCameraPosition.x;
            cameraPosition.y = tempCameraPosition.y;

            transform.position = cameraPosition;
        }
    }
}