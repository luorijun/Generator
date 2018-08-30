using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public MeshFilter filter;
    new public MeshRenderer renderer;

    public int seed;

    public enum DrawMode { HeightMap, ColorMap }
    public DrawMode drawMode;

    public int resolution;

    public float scale;

    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    [Range(1, 10)]
    public int octaves;

    public Vector2 offsize;

    public bool autoUpdate;

    public RegionData[] regions;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public void Generate()
    {
        float[,] heightData = TerrainGenerator.GenerateHeightData(seed, resolution, resolution, scale, persistance, lacunarity, octaves, offsize);

        filter.sharedMesh = TerrainGenerator.GetTerrainMesh(heightData, heightMultiplier, heightCurve);

        Texture2D texture = null;
        if (drawMode == DrawMode.HeightMap)
            texture = TerrainGenerator.GetHeightMap(heightData);
        else if (drawMode == DrawMode.ColorMap)
            texture = TerrainGenerator.GetColorMap(heightData, regions);

        renderer.sharedMaterial.mainTexture = texture;
    }
}
