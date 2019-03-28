using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {
    public class Planet: MonoBehaviour {
        public Face Face;

        public float Radius;

        public int Level;
        public float VisitRate;

        private Dictionary<string, Face> Faces;

        private void Start() {
            Faces.Add("up", Instantiate(Face, transform).Init(
                    Vector3.up,
                    Level, Radius, VisitRate,
                    this, "Up"));
            Faces.Add("down", Instantiate(Face, transform).Init(
                    Vector3.down,
                    Level, Radius, VisitRate,
                    this, "Down"));
            Faces.Add("left", Instantiate(Face, transform).Init(
                    Vector3.left,
                    Level, Radius, VisitRate,
                    this, "Left"));
            Faces.Add("right", Instantiate(Face, transform).Init(
                    Vector3.right,
                    Level, Radius, VisitRate,
                    this, "Right"));
            Faces.Add("forward", Instantiate(Face, transform).Init(
                    Vector3.forward,
                    Level, Radius, VisitRate,
                    this, "Forward"));
            Faces.Add("back", Instantiate(Face, transform).Init(
                    Vector3.back,
                    Level, Radius, VisitRate,
                    this, "Back"));
        }
        private void Update() {

        }
    }
}
