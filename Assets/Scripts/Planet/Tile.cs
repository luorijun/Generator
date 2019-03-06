using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour {
    public MeshFilter Filter;
    public MeshRenderer Renderer;
    private NFace face;

    public const int RESOLUTION = 16;

    public float Size;
    public float SizePerCell;

    public float Is;
    public bool IsActive;

    public Mesh Mesh;

    public Tile Init(float size) {
        Size = size;
        SizePerCell = Size / (RESOLUTION - 1);

        Mesh = CreateMesh();
        return this;
    }

    private Mesh CreateMesh() {
        var vertices = new Vector3[RESOLUTION * RESOLUTION];
        var triangles = new int[(RESOLUTION - 1) * (RESOLUTION - 1) * 6];

        var triIndex = 0;
        for (int y = 0; y < RESOLUTION; y++) {
            for (int x = 0; x < RESOLUTION; x++) {
                var i = x + y * RESOLUTION;

                vertices[i] = new Vector3(x * SizePerCell - Size / 2, 0, y * SizePerCell - Size / 2);
                if (x < RESOLUTION - 1 && y < RESOLUTION - 1) {
                    triangles[triIndex++] = i;
                    triangles[triIndex++] = i + RESOLUTION;
                    triangles[triIndex++] = i + RESOLUTION + 1;

                    triangles[triIndex++] = i + RESOLUTION + 1;
                    triangles[triIndex++] = i + 1;
                    triangles[triIndex++] = i;
                }
            }
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        return mesh;
    }
}
