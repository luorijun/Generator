using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {
    public class NPlanet: MonoBehaviour {
        public NFace Face;

        public float Radius;

        public int Level;
        public float VisitRate;

        private NFace[] Faces;

        private void Start() {
            Faces = new[]{
                Instantiate(Face,transform).Init(
                    Vector3.up,
                    Level,Radius,VisitRate,
                    this,"Up"),
                Instantiate(Face,transform).Init(
                    Vector3.down,
                    Level,Radius,VisitRate,
                    this,"Down"),
                Instantiate(Face,transform).Init(
                    Vector3.left,
                    Level,Radius,VisitRate,
                    this,"Left"),
                Instantiate(Face,transform).Init(
                    Vector3.right,
                    Level,Radius,VisitRate,
                    this,"Right"),
                Instantiate(Face,transform).Init(
                    Vector3.forward,
                    Level,Radius,VisitRate,
                    this,"Forward"),
                Instantiate(Face,transform).Init(
                    Vector3.back,
                    Level,Radius,VisitRate,
                    this,"Back"),
            };
        }
        private void Update() {

        }
    }
}
