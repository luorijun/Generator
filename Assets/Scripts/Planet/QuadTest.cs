using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Terrain;

public class QuadTest : MonoBehaviour {

    public int MaxDepth;
    public float Dis;

    private QuadTree tree;

	void Start () {
        tree = new QuadTree(MaxDepth, Dis);
        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = tree.GetMesh();
	}
	
	void Update () {
		
	}
}
