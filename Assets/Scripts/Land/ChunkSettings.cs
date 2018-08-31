using System;
using UnityEngine;

[Serializable]
public struct ChunkSettings
{
    public int seed;

    public int size;
    public float scale;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    [Range(1, 10)]
    public int octaves;

    public Vector2 offset;

    public RegionData[] regions;

    [Range(0, 4)]
    public int levelOfDetail;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
}

[Serializable]
public struct RegionData
{
    public string name;
    public float height;
    public Color color;
}