using LibNoise;
using LibNoise.Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempTest: MonoBehaviour {
    public Gradient gradient;

    public float north;
    public float south;
    public float west;
    public float east;

    private Noise2D builder;

    private void Start() {

      
        var noise = new Perlin();
        noise.Seed = 123;
        builder = new Noise2D(512, 256, noise);

        builder.GenerateSpherical(south, north, west, east);
        GetComponent<MeshRenderer>().material.mainTexture = builder.GetTexture(GradientPresets.Terrain);
        
    }
}
