using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Terrain;

public class QuadTest : MonoBehaviour {

    public int MaxDepth;
    public int Size;

    public float Dis;

    private QuadTree tree;

	void Start () {
        tree = new QuadTree(MaxDepth, Dis);

        GetComponent<MeshFilter>().sharedMesh = tree.GetMesh(Size);
    }
	
	void Update () {
        GetComponent<MeshFilter>().sharedMesh = tree.GetMesh(Size);
    }
}
