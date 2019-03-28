using System;
using UnityEngine;

namespace Generator.Utility {

    public class MeshFactory {

        public int resolution;
        public Texture2D texture;
        public Mesh mesh;

        public float[] heightData;
        public Color[] colorData;

        public Vector3[] vertices;
        public Vector2[] uv;
        public float[] triangles;

        public Action<int, int> CoordSender;
        public Func<float> HeightCalculator;
        public Func<Color> ColorCalculator;
        public Func<Vector3> VertexCalculator;

        public MeshFactory() {

        }

        public void Compute() {
            heightData = new float[resolution * resolution];
            colorData = new Color[heightData.Length];

            var offset = 0;
            for (int y = 0; y < resolution; y++) {
                for (int x = 0; x < resolution; x++) {
                    offset = y * resolution + x;

                    CoordSender(x, y);

                    // 高度数据
                    heightData[offset] = HeightCalculator();

                    // 纹理数据
                    colorData[offset] = ColorCalculator();

                    // 网格数据
                }
            }
        }
        public void Compute(int resolution) {
            this.resolution = resolution;
            Compute();
        }

        public static Mesh CreatePlane(int resolution, Func<int, int, Vector3> CalculateVertex) {
            var vertices = new Vector3[resolution * resolution];
            var uv = new Vector2[vertices.Length];
            var triangles = new int[(resolution - 1) * (resolution - 1) * 6];

            var triIndex = 0;
            for (int y = 0; y < resolution; y++) {
                for (int x = 0; x < resolution; x++) {
                    var i = y * resolution + x;

                    vertices[i] = CalculateVertex(x, y);
                    uv[i] = new Vector2((float)x / (resolution - 1), (float)y / (resolution - 1));

                    if (x < resolution - 1 && y < resolution - 1) {
                        triangles[triIndex++] = i;
                        triangles[triIndex++] = i + resolution;
                        triangles[triIndex++] = i + resolution + 1;

                        triangles[triIndex++] = i + resolution + 1;
                        triangles[triIndex++] = i + 1;
                        triangles[triIndex++] = i;
                    }
                }
            }

            var mesh = new Mesh {
                vertices = vertices,
                uv = uv,
                triangles = triangles,
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}
