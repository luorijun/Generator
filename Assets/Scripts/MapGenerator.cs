using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh }
    public DrawMode drawMode;

    public const int ChunkSize = 241;
    [Range(0, 6)]
    public int LevelOfDetail;
    public float Scale;

    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;

    public int Seed;
    public Vector2 Offset;

    public float MeshHeightMultiplier;
    public AnimationCurve HeightCurve;

    public bool AutoUpdate;

    public TerrainType[] Regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(ChunkSize, ChunkSize, Seed, Scale, Octaves, Persistance, Lacunarity, Offset);

        Color[] colorMap = new Color[ChunkSize * ChunkSize];
        for (int y = 0; y < ChunkSize; y++)
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        colorMap[y * ChunkSize + x] = Regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = GetComponent<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, ChunkSize, ChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeightMultiplier, HeightCurve, LevelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, ChunkSize, ChunkSize));
    }

    void OnValidate()
    {
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