using Generator.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translate: MonoBehaviour {

    public int resolution = 16;
    public int size = 1;

    public Vector3 BaseUp = Vector3.up;
    public Vector3 BaseDown = Vector3.down;
    public Vector3 BaseLeft = Vector3.left;
    public Vector3 BaseRight = Vector3.right;
    public Vector3 BaseForward = Vector3.forward;
    public Vector3 BaseBack = Vector3.back;

    private void Start() {
        var obj = new GameObject("generate",
            typeof(MeshFilter),
            typeof(MeshRenderer));
        obj.GetComponent<MeshFilter>().mesh =
            MeshFactory.CreatePlane(resolution,
                (x, z) => new Vector3 {
                    x = x * BaseForward.x,
                    y = 1 * BaseForward.y,
                    z = z * BaseForward.z
                });
    }

    private void Update() {

    }
}
