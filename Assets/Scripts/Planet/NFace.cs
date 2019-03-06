using Generator.DataStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {
    public class NFace: MonoBehaviour {
        public Tile Tile;

        public int Level;
        public float Size;
        public float VisitRate;

        private QuadTree<Tile> QuadTree;

        private void Start() {
            QuadTree = new QuadTree<Tile>(
                Level, node => {
                    var offset = new Vector3();
                    var size = Size / Mathf.Pow(2, node.Depth - 1);
                    switch (node.Mode) {
                        case QuadNode<Tile>.NodeMode.TopLeft:
                            offset = new Vector3(-1, 0, 1);
                            break;
                        case QuadNode<Tile>.NodeMode.TopRight:
                            offset = new Vector3(1, 0, 1);
                            break;
                        case QuadNode<Tile>.NodeMode.BottomLeft:
                            offset = new Vector3(-1, 0, -1);
                            break;
                        case QuadNode<Tile>.NodeMode.BottomRight:
                            offset = new Vector3(1, 0, -1);
                            break;
                    }
                    Tile tile = Instantiate(Tile,
                        offset * (size / 2) + (
                            node.Mode != QuadNode<Tile>.NodeMode.Full ?
                            node.Parent.Value.transform.position :
                            Vector3.zero),
                        Quaternion.identity,
                        transform)
                            .Init(size);
                    return tile;
                });
        }

        private void Update() {
            QuadTree.Traversing(
                node => VisitRate < Vector3.Distance(
                            Camera.main.transform.position,
                            node.Value.transform.position)
                        / node.Value.Size,
                node => node.Value.Filter.mesh = node.Value.Mesh,
                node => node.Value.Filter.mesh = null);
        }
    }
}