using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Terrain
{
    public class QuadTree
    {
        public QuadNode Root;

        public int MaxDepth;
        public int MaxSize;
        public float Dis;

        public Vector3[] vertices;
        public int[] triangles;

        public QuadTree(int maxDepth, float dis)
        {

            MaxDepth = maxDepth;
            MaxSize = (int)Mathf.Pow(2, maxDepth) + 1;
            Dis = dis;

            Root = new QuadNode(NodeType.FullNode, 1, this);

            vertices = InitVertices();
            triangles = UpdateTriangles();
        }

        public int[] UpdateTriangles()
        {
            return Root.Update().ToArray();
        }

        public Mesh GetMesh(int Size)
        {
            var mesh = new Mesh();

            mesh.vertices = vertices;
            mesh.triangles = UpdateTriangles();

            mesh.RecalculateNormals();
            return mesh;
        }

        private Vector3[] InitVertices()
        {
            var vertices = new Vector3[(int)Mathf.Pow(MaxSize, 2)];
            for (int y = 0; y < MaxSize; y++)
            {
                for (int x = 0; x < MaxSize; x++)
                {
                    var index = x + y * MaxSize;
                    vertices[index] = new Vector3(x, 0, y);
                }
            }
            return vertices;
        }
    }

    public enum NodeType
    {
        FullNode,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
