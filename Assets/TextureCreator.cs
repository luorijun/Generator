using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCreator: MonoBehaviour {
    public MeshRenderer Mesh;

    [Range(2, 512)]
    public int Resolution = 256;
    public float frequency = 10;

    [Range(1, 3)]
    public int dimensions = 3;

    private Texture2D texture;

    private void OnEnable() {

        if (texture == null) {
            texture = new Texture2D(Resolution, Resolution, TextureFormat.RGB24, true);
            texture.name = "Procedural Texture";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Trilinear;
            texture.anisoLevel = 9;

            Mesh.material.mainTexture = texture;
        }
        FillTexture();
    }

    private void Update() {

        if (transform.hasChanged) {
            FillTexture();
            transform.hasChanged = false;
        }
    }

    public void FillTexture() {
        if (texture.width != Resolution)
            texture.Resize(Resolution, Resolution);

        Vector3 tl = transform.TransformPoint(new Vector3(-.5f, -.5f));
        Vector3 tr = transform.TransformPoint(new Vector3(.5f, -.5f));
        Vector3 bl = transform.TransformPoint(new Vector3(-.5f, .5f));
        Vector3 br = transform.TransformPoint(new Vector3(.5f, .5f));

        for (int y = 0; y < Resolution; y++) {
            Vector3 l = Vector3.Lerp(tl, bl, (float)y / Resolution);
            Vector3 r = Vector3.Lerp(tr, br, (float)y / Resolution);
            for (int x = 0; x < Resolution; x++) {
                Vector3 point = Vector3.Lerp(l, r, (float)x / Resolution);
                texture.SetPixel(x, y, Color.white * Noise.noiseMethods[dimensions - 1](point, frequency));
            }
        }
        texture.Apply();
    }
}
