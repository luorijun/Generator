using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct GenerateSetting
{
    // 随机种子
    public int seed;

    // 地图大小，缩放
    [Space(10)]
    [Range(17, 241, order = 16)]
    public int size;
    public float scale;

    // 持续度，空隙度，阶度
    [Space(10)]
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    [Range(1, 10)]
    public int octaves;

    // 偏移
    [Space(10)]
    public Vector2 offset;

    // 绘制模式
    [Space(10)]
    public DrawMode drawMode;
    // 地区（划分高度）
    public RegionData[] regions;

    // 地形精度，相对高度，高度曲线
    [Space(10)]
    [Range(0, 4)]
    public int levelOfDetail;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
}

[Serializable]
public enum DrawMode
{
    HeightMap, ColorMap
}

[Serializable]
public struct RegionData
{
    public string name;
    public float height;
    public Color color;
}