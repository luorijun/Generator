using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{

    GameObject gameObject;

    Transform parent;
    public Vector3 face;

    public Vector3 axis1, axis2;

    public Face(Transform parent, Vector3 face)
    {
        this.parent = parent;
        this.face = face;

        gameObject = new GameObject(face.ToString(),
            typeof(MeshFilter),
            typeof(MeshRenderer));
        gameObject.transform.parent = parent;
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

        axis1 = new Vector3(face.y, face.z, face.x);
        axis2 = Vector3.Cross(face, axis1);
    }

    public void CreateMesh(int resolution)
    {
        var vertices = new Vector3[resolution * resolution];
        var triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        var triIndex = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                var i = x + resolution * y;

                var percent = new Vector2(x, y) / (resolution - 1);
                var pointOnUnitCube = face + (percent.x - .5f) * 2 * axis1 + (percent.y - .5f) * 2 * axis2;
                var pointOnUnitSphere = pointOnUnitCube.normalized;

                vertices[i] = pointOnUnitSphere;

                if (x < resolution - 1 && y < resolution - 1)
                {
                    triangles[triIndex++] = i;
                    triangles[triIndex++] = i + resolution + 1;
                    triangles[triIndex++] = i + resolution;

                    triangles[triIndex++] = i;
                    triangles[triIndex++] = i + 1;
                    triangles[triIndex++] = i + resolution + 1;
                }
            }
        }

        var mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.name = "Generate";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
