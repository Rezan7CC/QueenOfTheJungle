using System.Collections.Generic;
using UnityEngine;

namespace QJUtility
{ 
    public static class Primitives
    {
        public enum EPrimitiveTypeAdvanced
        {
            Capsule = PrimitiveType.Capsule,
            Cube = PrimitiveType.Cube,
            Cylinder = PrimitiveType.Cylinder,
            Plane = PrimitiveType.Plane,
            Quad = PrimitiveType.Quad,
            Sphere = PrimitiveType.Sphere,
            HalfCircle = 7
        }

        private static Dictionary<EPrimitiveTypeAdvanced, Mesh> primitiveMeshes
            = new Dictionary<EPrimitiveTypeAdvanced, Mesh>();

        public static GameObject CreatePrimitive(PrimitiveType type, bool withCollider)
        {
            return CreatePrimitive((EPrimitiveTypeAdvanced)type, withCollider);
        }

        public static GameObject CreatePrimitive(EPrimitiveTypeAdvanced type, bool withCollider)
        {
            if (withCollider)
                return GameObject.CreatePrimitive((PrimitiveType)type);

            GameObject gameObject = new GameObject(type.ToString());
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = Primitives.GetPrimitiveMesh(type);
            gameObject.AddComponent<MeshRenderer>();

            return gameObject;
        }

        public static Mesh GetPrimitiveMesh(PrimitiveType type)
        {
            return GetPrimitiveMesh((EPrimitiveTypeAdvanced)type);
        }

        public static Mesh GetPrimitiveMesh(EPrimitiveTypeAdvanced type)
        {
            if (!Primitives.primitiveMeshes.ContainsKey(type) 
              || Primitives.primitiveMeshes[type] == null)
            {
                if (IsAdvandedPrimitiveType(type))
                    Primitives.CreatePrimitiveMeshAdvanced(type);
                else
                    Primitives.CreatePrimitiveMesh((PrimitiveType)type);
            }

            return Primitives.primitiveMeshes[type];
        }

        private static bool IsAdvandedPrimitiveType(EPrimitiveTypeAdvanced type)
        {
            switch ((PrimitiveType)type)
            {
                case PrimitiveType.Capsule:
                case PrimitiveType.Cube:
                case PrimitiveType.Cylinder:
                case PrimitiveType.Plane:
                case PrimitiveType.Quad:
                case PrimitiveType.Sphere:
                    return false;
                default:
                    return true;
            }
        }

        private static Mesh CreatePrimitiveMesh(PrimitiveType type)
        {
            GameObject gameObject = GameObject.CreatePrimitive(type);
            Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            GameObject.DestroyImmediate(gameObject);

            Primitives.primitiveMeshes[(EPrimitiveTypeAdvanced)type] = mesh;
            return mesh;
        }

        private static Mesh CreatePrimitiveMeshAdvanced(EPrimitiveTypeAdvanced type)
        {
            Mesh mesh = null;
            switch(type)
            {
                case EPrimitiveTypeAdvanced.HalfCircle:
                {
                    mesh = CreateHalfCircle();
                    break;
                }
                default:
                    return null;
            }
            Primitives.primitiveMeshes[type] = mesh;
            return mesh;
        }

        private static Mesh CreateHalfCircle()
        {
            Mesh halfCircle = new Mesh();
            halfCircle.Clear();

            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector3> normals = new List<Vector3>();
            int vertexNum = 25;
            float angleStep = 360.0f / (vertexNum * 2);
            float currentAngle = 0;
            float distance = 1;

            for(int vertexIndex = 0; vertexIndex < vertexNum + 1; vertexIndex++)
            {
                Vector3 direction = Quaternion.Euler(0, 0, currentAngle) * Vector3.right;
                Vector3 vertex = direction * distance;
                vertices.Add(vertex);
                indices.Add(vertexIndex);
                normals.Add(direction);

                if (vertexIndex + 1 <= vertexNum)
                    indices.Add(vertexIndex + 1);
                else
                    indices.Add(0);

                currentAngle += angleStep;
            }

            halfCircle.SetVertices(vertices);
            halfCircle.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            halfCircle.SetNormals(normals);

            return halfCircle;
        }    
    }
}
