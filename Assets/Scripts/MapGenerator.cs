using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh }
    public DrawMode drawMode;

    public int Width;
    public int Height;
    public float Scale;

    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;

    public int Seed;
    public Vector2 Offset;

    public bool AutoUpdate;

    public TerrainType[] Regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(Width, Height, Seed, Scale, Octaves, Persistance, Lacunarity, Offset);

        Color[] colorMap = new Color[Width * Height];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        colorMap[y * Width + x] = Regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = GetComponent<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, Width, Height));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorMap, Width, Height));
    }

    void OnValidate()
    {
        Width = Mathf.Max(1, Width);
        Height = Mathf.Max(1, Height);
        Lacunarity = Mathf.Max(1, Lacunarity);
        Octaves = Mathf.Max(0, Octaves);
    }

}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}