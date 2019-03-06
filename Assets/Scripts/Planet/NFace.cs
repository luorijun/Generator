using Generator.DataStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFace: MonoBehaviour {
    public Tile Tile;

    public int Level;
    public float Size;
    public float VisitRate;

    private QuadTree<Tile> QuadTree;

    private void Start() {
        QuadTree = new QuadTree<Tile>(
            Level, depth => Instantiate(Tile, transform).Init(Size / Mathf.Pow(2, depth - 1)));
    }

    private void Update() {
        QuadTree.Traversing(
            node => {
                var ta = Vector3.Distance(
                        Camera.main.transform.position,
                        node.Value.transform.position)
                    / node.Value.Size;

                var tb = VisitRate < ta;

                node.Value.Is = ta;
                node.Value.IsActive = tb;
                return tb;
            },
            node => node.Value.Filter.mesh = node.Value.Mesh,
            node => node.Value.Filter.mesh = null);
    }
}
