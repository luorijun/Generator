using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshFilter Filter;
    public MeshRenderer Renderer;
    private NFace face;

    public const int RESOLUTION = 16;

    public int depth;
    private float size;

    // children
    private Tile ChildTopLeft;
    private Tile ChildTopRight;
    private Tile ChildBottomLeft;
    private Tile ChildBottomRight;

    void Start()
    {
        if (depth < face.Level)
        {
            ChildTopLeft = Instantiate(face.tile, transform.position, Quaternion.identity, transform).Init(face, depth + 1);
            ChildTopRight = Instantiate(face.tile, transform.position + Vector3.right * size, Quaternion.identity, transform).Init(face, depth + 1);
            ChildBottomLeft = Instantiate(face.tile, transform.position + Vector3.back * size, Quaternion.identity, transform).Init(face, depth + 1);
            ChildBottomRight = Instantiate(face.tile, transform.position + new Vector3(1, 0, -1) * size, Quaternion.identity, transform).Init(face, depth + 1);
        }
        else
        {
            Filter.mesh = CreateMesh();
        }
    }

    void Update()
    {

    }

    public Tile Init(NFace face, int depth)
    {
        this.face = face;
        this.depth = depth;
        size = face.Size / 2 / Mathf.Pow(2, depth - 1);
        return this;
    }

    private Mesh CreateMesh()
    {
        var vertices = new Vector3[RESOLUTION * RESOLUTION];
        var triangles = new int[(RESOLUTION - 1) * (RESOLUTION - 1) * 6];

        var triIndex = 0;
        for (int y = 0; y < RESOLUTION; y++)
        {
            for (int x = 0; x < RESOLUTION; x++)
            {
                var i = x + y * RESOLUTION;

                vertices[i] = new Vector3(x, 0, y);
                if (x < RESOLUTION - 1 && y < RESOLUTION - 1)
                {
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
