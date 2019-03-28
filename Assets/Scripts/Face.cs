using LibNoise.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {


    public class Face: MonoBehaviour {

        public Material material;

        public Matrix4x4 matrix;

        public float radius;
        public float maxDepth;
        public Perlin perlin;
        public float frequency;
        public float amplitude;

        public bool isNoise;

        private Chunk root;
        private Sphere planet;

        public Face Init(Matrix4x4 matrix, Sphere planet) {
            material = planet.material;
            this.matrix = matrix;
            radius = planet.radius;
            maxDepth = planet.detail;
            perlin = planet.perlin;
            frequency = planet.frequency;
            amplitude = planet.amplitude;

            isNoise = planet.isNoise;

            root = Chunk.New(
                planet.radius*2, 1,
                Vector3.up*planet.radius,
                null, this, transform);
            this.planet = planet;
            return this; }

        public void UpdateState() {
            root.UpdateState(planet.viewer, planet.subdivisionRatio);
        }

        public static Face New(Matrix4x4 matrix, Sphere parent) {
            var face = new GameObject().AddComponent<Face>();
            face.transform.SetParent(parent.transform);
            face.name = "face";
            return face.Init(matrix, parent);
        }
    }
}

