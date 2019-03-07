using Generator.DataStructure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {
    public class NFace: MonoBehaviour {
        public Tile Tile;
        public NPlanet Planet;

        public Vector3 FaceTo;
        public Vector3 XAxis;
        public Vector3 ZAxis;

        public int Level;
        public float Size;
        public float VisitRate;

        private QuadTree<Tile> QuadTree;

        private void Start() {
            QuadTree = new QuadTree<Tile>(Level,
                node => {
                    var offset = new Vector3();
                    var size = Size / Mathf.Pow(2, node.Depth - 1);
                    switch (node.Mode) {
                        case QuadNode<Tile>.NodeMode.TopLeft:
                            offset = XAxis * -1 + ZAxis * 1;
                            break;
                        case QuadNode<Tile>.NodeMode.TopRight:
                            offset = XAxis * 1 + ZAxis * 1;
                            break;
                        case QuadNode<Tile>.NodeMode.BottomLeft:
                            offset = XAxis * -1 + ZAxis * -1;
                            break;
                        case QuadNode<Tile>.NodeMode.BottomRight:
                            offset = XAxis * 1 + ZAxis * -1;
                            break;
                    }
                    Tile tile = Instantiate(Tile,
                        offset * (size / 2) + (
                            node.Mode != QuadNode<Tile>.NodeMode.Full ?
                            node.Parent.Value.transform.position :
                            FaceTo * Planet.Radius),
                        Quaternion.identity,
                        transform)
                            .Init(size, this);
                    return tile;
                });
        }

        private void Update() {
            QuadTree.Traversing(
                node => VisitRate < Vector3.Distance(
                            Camera.main.transform.position,
                            node.Value.transform.position)
                        / node.Value.Size,
                node => node.Value.gameObject.SetActive(true),
                node => node.Value.gameObject.SetActive(false));
        }

        public NFace Init(Vector3 faceTo, int level, float radius, float visitRate, NPlanet planet, string name = "face") {
            this.name = name;

            FaceTo = faceTo;
            Level = level;
            Size = radius * 2;
            VisitRate = visitRate;
            Planet = planet;

            XAxis = new Vector3(FaceTo.y, FaceTo.z, FaceTo.x);
            ZAxis = Vector3.Cross(FaceTo, XAxis);
            return this;
        }
    }
}