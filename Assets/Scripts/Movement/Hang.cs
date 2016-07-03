using UnityEngine;
using System.Collections;

namespace QJMovement
{
    [RequireComponent(typeof(MovementController))]
    public class Hang : MonoBehaviour
    {
        [SerializeField]
        bool canHang = true;

        [SerializeField]
        float hangOffset = -1;

        [SerializeField]
        float interpDuration = 0.25f;

        bool isHanging;

        float currentHangOffset = 0;

        float currentTargetMaxLeft = 0;

        float currentTargetMaxRight = 0;

        bool isInterpolating = false;

        MovementController movementController = null;

        public bool CanHang
        {
            get { return canHang; }
            set { canHang = value; }
        }

        public bool IsHanging
        {
            get { return isHanging; }
        }

        public bool IsInterpolating
        {
            get { return isInterpolating; }
        }

        void Awake()
        {
            movementController = GetComponent<MovementController>();
            if(movementController == null)
                Debug.LogError("MovementController not found!");
        }

        void LateUpdate()
        {
            if(isHanging || isInterpolating)
            {
                Vector3 position = transform.position;
                position.x = Mathf.Clamp(position.x, currentTargetMaxLeft, currentTargetMaxRight);
                transform.position = position;
            }
        }

        public void ActivateHanging()
        {
            if (isHanging)
                return;

            Collider2D hangTargetCollider = movementController.IsGroundedOn(movementController.OneWayPlatformMask);
            if (hangTargetCollider == null)
                return;

            currentTargetMaxLeft = hangTargetCollider.bounds.min.x;
            currentTargetMaxRight = hangTargetCollider.bounds.max.x;

            isHanging = true;
            StartCoroutine(HangingInterpolation(isHanging));
        
        }

        public void DeactiveHanging()
        {
            isHanging = false;
            StartCoroutine(HangingInterpolation(isHanging));
        }

        IEnumerator HangingInterpolation(bool active)
        {
            if (active && movementController != null)
            {
                movementController.GravityEnabled = false;
                movementController.IgnoreOneWayPlatforms = true;

                Vector2 velocity = movementController.Velocity;
                velocity.y = 0;
                movementController.Velocity = velocity;
            }

            float startHangOffset = currentHangOffset;
            float currentDuration = 0;
            while(currentDuration <= interpDuration && active == isHanging)
            {
                isInterpolating = true;
                currentDuration += Time.deltaTime;
                float oldHangOffset = currentHangOffset;
                float t = Mathf.Clamp01(currentDuration / interpDuration);
                currentHangOffset = Mathf.Lerp(0, active ? hangOffset : startHangOffset, active ? t : 1.0f - t);

                Vector3 tempPosition = transform.position;
                tempPosition.y -= oldHangOffset;
                tempPosition.y += currentHangOffset;
                transform.position = tempPosition;
                yield return new WaitForEndOfFrame();
            }

            if (active == isHanging)
            {
                if (!active && movementController != null)
                {
                    movementController.GravityEnabled = true;
                    movementController.IgnoreOneWayPlatforms = false;
                }
            }
            isInterpolating = false;
        }
    }
}
