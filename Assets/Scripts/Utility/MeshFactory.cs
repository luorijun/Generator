using System;
using UnityEngine;

namespace Generator.Utility {

    public class MeshFactory {

        public static Mesh CreatePlane(int resolution, Func<int, int, Vector3> CalculateVertex) {
            var vertices = new Vector3[resolution * resolution];
            var triangles = new int[(resolution - 1) * (resolution - 1) * 6];

            var triIndex = 0;
            for (int y = 0; y < resolution; y++) {
                for (int x = 0; x < resolution; x++) {
                    var i = y * resolution + x;

                    vertices[i] = CalculateVertex(x, y);
                    if (x < resolution - 1 && y < resolution - 1) {
                        triangles[triIndex++] = i;
                        triangles[triIndex++] = i + resolution + 1;
                        triangles[triIndex++] = i + resolution;

                        triangles[triIndex++] = i + resolution + 1;
                        triangles[triIndex++] = i;
                        triangles[triIndex++] = i + 1;
                    }
                }
            }

            var mesh = new Mesh {
                vertices = vertices,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            return mesh;
        }

        private static void GetData() {
        }
    }
}
