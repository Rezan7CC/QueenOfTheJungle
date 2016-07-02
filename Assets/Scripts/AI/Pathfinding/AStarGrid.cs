using UnityEngine;
using System.Collections;
using System;

namespace QJAI.QJPathfinding
{
    [ExecuteInEditMode]
    public class AStarGrid : MonoBehaviour
    {
        public struct GridNode
        {
            public Vector3 position;

            public void Draw()
            {
                float lineLength = 0.1f;

                Gizmos.color = Color.green;

                Gizmos.DrawLine(position + new Vector3(-lineLength, 0),
                                position + new Vector3(lineLength, 0));

                Gizmos.DrawLine(position + new Vector3(0, lineLength),
                                position + new Vector3(0, -lineLength));                
            }
        }

        [System.Serializable]
        public struct Vector2UInt
        {
            public Vector2UInt(uint x, uint y)
            {
                this.x = x;
                this.y = y;
            }

            public static Vector2UInt operator *(Vector2UInt left, Vector2UInt right)
            {
                return new Vector2UInt(left.x * right.x, left.y * right.y);
            }

            public uint x;
            public uint y;
        }

        [SerializeField]
        bool updateNodesNow = false;
        [SerializeField]
        bool showGridNodes = true;
        [SerializeField]
        Vector2UInt gridSize = new Vector2UInt(8, 8);
        [SerializeField]
        Vector2UInt gridNodeResolution = new Vector2UInt(8, 8);

        GridNode[,] gridNodes = null;

        float drawNodesTime = 1.5f;
        float currentDrawNodesTime = 0;

        Transform cachedTransform = null;

        bool nodeGizmosDirty = true;

        void Awake()
        {
            cachedTransform = transform;
        }

        // Use this for initialization
        void Start()
        {
            UpdateNodes();
        }

        // Update is called once per frame
        void Update()
        {
            //// Move to button in some editor script
            //if (gridNodes == null || updateNodesNow)
            //{
            //    updateNodesNow = false;
            //    UpdateNodes();
            //}

            //// Move to button in some editor script
            //if (showGridNodes)
            //{
            //    currentDrawNodesTime -= Time.deltaTime;
            //    if (currentDrawNodesTime <= 0)
            //    {
            //        currentDrawNodesTime = drawNodesTime;
            //        DrawNodes(drawNodesTime);
            //    }
            //}
        }

        public void OnDrawGizmosSelected()
        {
            if (showGridNodes)
                DrawNodes();
        }

        public void UpdateNodes()
        {
            Vector2UInt gridNodesNumber = gridSize * gridNodeResolution;

            gridNodes = new GridNode[gridNodesNumber.x,
                                     gridNodesNumber.y];

            for (uint x = 0; x < gridNodesNumber.x; x++)
            {
                for (uint y = 0; y < gridNodesNumber.y; y++)
                {
                    gridNodes[x, y].position = cachedTransform.position +
                                               new Vector3((float)x / gridNodeResolution.x - (float)gridSize.x * 0.5f,
                                                           (float)y / gridNodeResolution.y, 0);
                }
            }
            nodeGizmosDirty = true;
        }

        public void DrawNodes()
        {
            if (gridNodes != null)
            {
                Vector2UInt gridNodesNumber = gridSize * gridNodeResolution;

                for (uint x = 0; x < gridNodesNumber.x; x++)
                {
                    for (uint y = 0; y < gridNodesNumber.y; y++)
                    {
                        gridNodes[x, y].Draw();
                    }
                }
            }
        }
    }
}