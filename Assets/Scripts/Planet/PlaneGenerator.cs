using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlaneGenerator
{
    public static Texture GetHeightMap(int size)
    {
        var tex = new Texture2D(size, size);
        var weight = new Color[size * size];
        for (int i = 0; i < weight.Length; i++)
        {
            float x = i % size;
            float y = i / size;
            var tmp = Mathf.PerlinNoise(x / size, y / size);
            weight[i] = new Color(tmp, tmp, tmp);
        }
        tex.SetPixels(weight);
        tex.Apply();
        return tex;
    }
    public static Mesh GetPlane(int size)
    {
        var vertices = new Vector3[size * size];
        var uvs = new Vector2[size * size];
        var triangles = new int[(size - 1) * (size - 1) * 6];

        var triIndex = 0;
        for (int i = 0; i < size * size; i++)
        {
            vertices[i] = new Vector3(i % size, 0, i / size);
            uvs[i] = new Vector2(i % size / (float)size, i / size / (float)size);

            if (i % size < size - 1 && i / size < size - 1)
            {
                triangles[triIndex++] = i;
                triangles[triIndex++] = i + size;
                triangles[triIndex++] = i + size + 1;
                triangles[triIndex++] = i + size + 1;
                triangles[triIndex++] = i + 1;
                triangles[triIndex++] = i;
            }
        }

        var mesh = new Mesh();
        mesh.name = "generate";
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        return mesh;
    }

    public static Mesh CreateMesh(int size, int resolution, Vector3 direction)
    {


        var mesh = new Mesh();
        return mesh;
    }
}
