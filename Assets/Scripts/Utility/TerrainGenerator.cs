using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Generator.Util {

    [Serializable]
    public class TerrainGenerator {

        #region Fields
        private int size;
        private int resolution;

        public double[] texData;
        private Color[] heights;
        public Color[] normals;

        [SerializeField]
        public Texture2D normal;
        [SerializeField]
        public Texture2D height;
        #endregion

        #region Properties
        public int Size { get; set; }
        public int Resolition
        {
            get
            {
                return resolution;
            }
            set
            {
                var length = value * value;

                texData = new double[length];
                normals = new Color[length];
                heights = new Color[length];

                resolution = value;
            }
        }
        #endregion

        #region Constructors
        public TerrainGenerator(int size, int resolution) {
            Size = size;
            Resolition = resolution;
        }
        #endregion

        public void ComputeTexture(Func<double, double, double> GetHeight) {

            int offset = 0;
            double u, v, u1, u2, v1, v2 = 0;
            double detail = 1.0 / resolution;
            double half = detail / 2;

            for (int y = 0; y < resolution; y++) {
                for (int x = 0; x < resolution; x++) {
                    offset = y * resolution + x;

                    u = x * detail;
                    v = y * detail;

                    u1 = GetHeight(u, v);
                    u2 = GetHeight(u + detail, v);
                    var tu = new Vector3((float)detail, (float)(u2 - u1), 0);

                    v1 = GetHeight(u, v);
                    v2 = GetHeight(u, v + detail);
                    var tv = new Vector3(0, (float)(v2 - v1), (float)detail);

                    texData[offset] = GetHeight(u + half, v + half);
                    heights[offset] = Color.white * (float)((texData[offset] + 1) / 2);

                    var v3 = Vector3.Cross(tv, tu).normalized;
                    normals[offset] = new Color((v3.x + 1) / 2, (v3.z + 1) / 2, (v3.y + 1) / 2, (v3.x + 1) / 2);
                }
            }
            DisplayTexture();
        }
        public void DisplayTexture() {
            normal = new Texture2D(resolution, resolution);
            height = new Texture2D(resolution, resolution);
            normal.SetPixels(normals);
            height.SetPixels(heights);
            normal.Apply();
            height.Apply();

            File.WriteAllBytes("Assets/Normal.png", normal.EncodeToPNG());
        }

        public void ComputeMesh() {

            var offset = 0;
            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    offset = y * size + x;

                }
            }
        }
    }
}