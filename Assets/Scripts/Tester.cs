using Generator.Util;
using LibNoise;
using LibNoise.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester: MonoBehaviour {

    public int size;
    public int resolution;

    public TerrainGenerator generator;

    private Perlin noise;

    private void Start() {
        noise = new Perlin(1, 2, .5, 8, 0, QualityMode.High);

        generator = new TerrainGenerator(size, resolution);
        generator.ComputeTexture((x, y) => {
            return noise.GetValue(x, y, 0);
        });
        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", generator.height);
        GetComponent<MeshRenderer>().material.SetTexture("_BumpMap", generator.normal);
        GetComponent<MeshRenderer>().material.SetTexture("_ParallaxMap", generator.height);
        GetComponent<MeshRenderer>().material.EnableKeyword("_NORMALMAP");
    }
}
