using UnityEngine;
using UnityEditor;
using System.Collections;
using QJUtility;

namespace QJAI.QJSensing
{
    [ExecuteInEditMode]
    public class AISensing : MonoBehaviour
    {
        enum EHalfCircleOrientation
        {
            Front, Back, Top, Bottom
        }

        enum EShowRadiiMode
        {
            Never, Always, Selected
        }

        [System.Serializable]
        struct SenseRadius
        {
            public string name;
            public float radius;
            public Color color;
            public bool onlyHalfCircle;
            public EHalfCircleOrientation halfCircleOrientation;

            public SenseRadius(string name, float radius, Color color)
            {
                this.name = name;
                this.radius = radius;
                this.color = color;
                onlyHalfCircle = false;
                halfCircleOrientation = EHalfCircleOrientation.Front;
            }
        }

        [SerializeField]
        SenseRadius[] senseRadii;

        [SerializeField]
        EShowRadiiMode showRadiiMode = EShowRadiiMode.Always;

        [SerializeField] [HideInInspector]
        bool firstDeserilization = true;

        void OnValidate()
        {
            if (firstDeserilization)
            {
                senseRadii = new SenseRadius[3];
                senseRadii[0] = new SenseRadius("DefaultRadius01", 1, Color.green);
                senseRadii[0].onlyHalfCircle = true;
                senseRadii[1] = new SenseRadius("DefaultRadius02", 1.2f, Color.red);
                senseRadii[2] = new SenseRadius("DefaultRadius03", 1.4f, Color.yellow);
                firstDeserilization = false;
            }
        }

        void Awake()
        {
            if (Application.isEditor)
                EditorUtility.SetDirty(this);
        }

        void OnDrawGizmos()
        {
            if (showRadiiMode == EShowRadiiMode.Always)
                DrawRadiiGizmos();
        }

        void OnDrawGizmosSelected()
        {
            if (showRadiiMode == EShowRadiiMode.Selected)
                DrawRadiiGizmos();
        }

        void DrawRadiiGizmos()
        {
            if (senseRadii == null || senseRadii.Length <= 0)
                return;

            foreach (SenseRadius senseRadius in senseRadii)
            {
                Gizmos.color = senseRadius.color;
                if(senseRadius.onlyHalfCircle)
                    Gizmos.DrawWireMesh(
                        Primitives.GetPrimitiveMesh(Primitives.EPrimitiveTypeAdvanced.HalfCircle),
                        transform.position,
                        OrientationToRotation(senseRadius.halfCircleOrientation),
                        new Vector3(senseRadius.radius,
                                    senseRadius.radius, 
                                    senseRadius.radius));
                else
                    Gizmos.DrawWireSphere(transform.position, senseRadius.radius);

            }
        }

        Quaternion OrientationToRotation(EHalfCircleOrientation orientation)
        {
            switch(orientation)
            {
                case EHalfCircleOrientation.Top:
                    return transform.rotation;
                case EHalfCircleOrientation.Bottom:
                    return transform.rotation * Quaternion.Euler(0, 0, 180);
                case EHalfCircleOrientation.Front:
                    return transform.rotation * Quaternion.Euler(0, 0, -90);
                case EHalfCircleOrientation.Back:
                    return transform.rotation * Quaternion.Euler(0, 0, 90);
                default:
                    return transform.rotation;
            }
        }
    }
}
