using Generator;
using LibNoise.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generator {
    public class Sphere: MonoBehaviour {
        private readonly Vector3[][] FaceTo = new[] {
            new []{ // up
                new Vector3( 1, 0, 0),
                new Vector3( 0, 1, 0),
                new Vector3( 0, 0, 1)},
            new []{ // down
                new Vector3( 1, 0, 0),
                new Vector3( 0,-1, 0),
                new Vector3( 0, 0,-1)},
            new []{ // left
                new Vector3( 0, 0, 1),
                new Vector3(-1, 0, 0),
                new Vector3( 0,-1, 0)},
            new []{ // right
                new Vector3( 0, 0, 1),
                new Vector3( 1, 0, 0),
                new Vector3( 0, 1, 0)},
            new []{ // forward
                new Vector3( 1, 0, 0),
                new Vector3( 0, 0,-1),
                new Vector3( 0, 1, 0)},
            new []{ // back
                new Vector3( 1, 0, 0),
                new Vector3( 0, 0, 1),
                new Vector3( 0,-1, 0)},
        };

        public Material material;

        public Transform viewer;

        public enum GenerateMode {
            ByRadius, ByDetail
        }
        public GenerateMode mode = GenerateMode.ByRadius;

        [Range(2, 241)]
        public int chunkSize = 16;
        public float radius = 500;
        public int detail = 6;
        public bool autoSet = true;

        public Perlin perlin;
        public SphereNoiseSettings NoiseSettings = new SphereNoiseSettings {
            frequency = 1,
            lacunarity = 2,
            persistence = .5,
            octaves = 6,
            seed = 0,
        };
        public float frequency = .002f;
        public float amplitude = .004f;

        public float subdivisionRatio = 1;

        public bool isNoise = true;

        private Face[] faces;

        private void Awake() {
            Chunk.Resolution = chunkSize;
            perlin = new Perlin(
                NoiseSettings.frequency,
                NoiseSettings.lacunarity,
                NoiseSettings.persistence,
                NoiseSettings.octaves,
                NoiseSettings.seed,
                LibNoise.QualityMode.High);

            if (autoSet) {
                switch (mode) {
                    case GenerateMode.ByRadius:
                        detail = Mathf.RoundToInt(Mathf.Log(radius, 2) - Mathf.Log(chunkSize / 2f, 2) + 1);
                        break;
                    case GenerateMode.ByDetail:
                        radius = chunkSize / 2f * Mathf.Pow(2, detail - 1);
                        break;
                }
            }

            material.SetFloat("_Min", radius * (1 - amplitude));
            material.SetFloat("_Max", radius * (1 + amplitude));
            UpdateState();

            faces = new Face[6];
            for (int i = 0; i < 6; i++) {
                faces[i] = Face.New(new Matrix4x4(
                    FaceTo[i][0], FaceTo[i][1], FaceTo[i][2], Vector4.zero), this);
            }
            transform.position = Vector3.down * radius;
        }

        private void Update() {
            UpdateState();
            foreach (var face in faces) {
                face.UpdateState();
            }
        }

        private void UpdateState() {
            // 更新坐标
            material.SetVector("_Center", transform.position);
        }
    }

    [System.Serializable]
    public struct SphereNoiseSettings {
        public double frequency;
        public double lacunarity;
        public double persistence;
        public int octaves;
        public int seed;
    }
}
