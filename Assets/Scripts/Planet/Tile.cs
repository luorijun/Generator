using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile: MonoBehaviour {
    public MeshFilter Filter;
    public MeshRenderer Renderer;
    private NFace face;

    public const int RESOLUTION = 16;

    public int depth;
    public float size;
    public float sizePerCell;
    public Vector3 center;

    public bool isSub;

    // children
    public bool HasChild;

    private Tile ChildTopLeft;
    private Tile ChildTopRight;
    private Tile ChildBottomLeft;
    private Tile ChildBottomRight;

    void Start() {
        if (depth < face.Level) {
            ChildTopLeft = Instantiate(face.tile, transform.position, Quaternion.identity, transform).Init(face, depth + 1);
            ChildTopRight = Instantiate(face.tile, transform.position + Vector3.right * size / 2, Quaternion.identity, transform).Init(face, depth + 1);
            ChildBottomLeft = Instantiate(face.tile, transform.position + Vector3.forward * size / 2, Quaternion.identity, transform).Init(face, depth + 1);
            ChildBottomRight = Instantiate(face.tile, transform.position + new Vector3(1, 0, 1) * size / 2, Quaternion.identity, transform).Init(face, depth + 1);
            HasChild = true;
        }
        else {
            Filter.mesh = CreateMesh();
        }
    }

    public Tile Init(NFace face, int depth) {
        this.face = face;
        this.depth = depth;
        UpdateData();
        return this;
    }

    public void UpdateState() {
        UpdateData();
        UpdateMesh();

        if (HasChild) {
            ChildTopLeft.UpdateState();
            ChildTopRight.UpdateState();
            ChildBottomLeft.UpdateState();
            ChildBottomRight.UpdateState();
        }
    }
    private void UpdateData() {
        size = face.Size / Mathf.Pow(2, depth - 1);
        sizePerCell = size / (RESOLUTION - 1);
        center = transform.position + new Vector3(1, 0, 1) * (size / 2);

        if (HasChild) {
            ChildTopRight.transform.localPosition = Vector3.right * size / 2;
            ChildBottomLeft.transform.localPosition = Vector3.forward * size / 2;
            ChildBottomRight.transform.localPosition = new Vector3(1, 0, 1) * size / 2;
        }
    }
    private void UpdateMesh() {

        isSub = face.ViewDistance < Vector3.Distance(Camera.main.transform.position, center) / size;

        if (!HasChild) {
            Debug.Log("depth: " + depth + ", create mesh");
            Filter.mesh = CreateMesh();
        }
    }

    private Mesh CreateMesh() {
        var vertices = new Vector3[RESOLUTION * RESOLUTION];
        var triangles = new int[(RESOLUTION - 1) * (RESOLUTION - 1) * 6];

        var triIndex = 0;
        for (int y = 0; y < RESOLUTION; y++) {
            for (int x = 0; x < RESOLUTION; x++) {
                var i = x + y * RESOLUTION;

                vertices[i] = new Vector3(x * sizePerCell, 0, y * sizePerCell);
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

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(center, 1);
    }
}
