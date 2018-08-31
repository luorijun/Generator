using UnityEngine;
using System;

public class LandGenerator
{
    public ChunkSettings ChunkSettings { get; set; }

    private float[,] heightData;

    public LandGenerator(ChunkSettings settings)
    {
        ChunkSettings = settings;

        heightData = new float[ChunkSettings.size, ChunkSettings.size];
    }

    public static float[,] CalculateHeightData(ChunkSettings chunkSettings)
    {
        float[,] heightData = new float[chunkSettings.size, chunkSettings.size];

        // 将每阶噪声图进行随机化偏移
        System.Random random = new System.Random(chunkSettings.seed);
        Vector2[] octaveOffsets = new Vector2[chunkSettings.octaves];
        for (int i = 0; i < chunkSettings.octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + chunkSettings.offset.x;
            float offsetY = random.Next(-100000, 100000) + chunkSettings.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        float halfWidth = chunkSettings.size / 2f;
        float halfHeight = chunkSettings.size / 2f;

        // 计算高度
        for (int y = 0; y < chunkSettings.size; y++)
        {
            for (int x = 0; x < chunkSettings.size; x++)
            {
                float amplitude = 1;
                float frequency = 1;

                // 阶度混合
                float totalHeight = 0;
                for (int i = 0; i < chunkSettings.octaves; i++)
                {
                    float xCoord = (x - halfWidth) / chunkSettings.scale * frequency + octaveOffsets[i].x;
                    float yCoord = (y - halfHeight) / chunkSettings.scale * frequency + octaveOffsets[i].y;

                    float currentHeight = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    totalHeight += currentHeight * amplitude;

                    amplitude *= chunkSettings.persistance;
                    frequency *= chunkSettings.lacunarity;
                }

                maxHeight = Mathf.Max(maxHeight, totalHeight);
                minHeight = Mathf.Min(minHeight, totalHeight);

                heightData[x, y] = totalHeight;
            }
        }

        // 规约为 0-1 之间的浮点数
        for (int y = 0; y < chunkSettings.size; y++)
        {
            for (int x = 0; x < chunkSettings.size; x++)
            {
                heightData[x, y] = Mathf.InverseLerp(minHeight, maxHeight, heightData[x, y]);
            }
        }

        return heightData;
    }
    public float[,] CalculateHeightData()
    {
        heightData = CalculateHeightData(ChunkSettings);
        return heightData;
    }

    public static Texture2D DrawHeightMap(float[,] heightData)
    {
        return DrawTexture(heightData, (h) =>
        {
            return new Color(h, h, h);
        });
    }
    public static Texture2D DrawHeightMap(ChunkSettings chunkSettings)
    {
        return DrawHeightMap(CalculateHeightData(chunkSettings));
    }
    public Texture2D DrawHeightMap()
    {
        return DrawHeightMap(heightData);
    }

    public static Texture2D DrawColorMap(float[,] heightData, RegionData[] regions)
    {
        return DrawTexture(heightData, (h) =>
        {
            for (int i = 0; i < regions.Length; i++)
            {
                if (h <= regions[i].height)
                    return regions[i].color;
            }
            return Color.white;
        });
    }
    public static Texture2D DrawColorMap(ChunkSettings chunkSettings)
    {
        return DrawColorMap(CalculateHeightData(chunkSettings), chunkSettings.regions);
    }
    public Texture2D DrawColorMap()
    {
        return DrawColorMap(heightData, ChunkSettings.regions);
    }

    public static Texture2D DrawTexture(float[,] heightData, Func<float, Color> DrawPixel)
    {
        int size = heightData.GetLength(0);

        Color[] colorData = new Color[(int)Mathf.Pow(size, 2)];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                colorData[y * size + x] = DrawPixel(heightData[x, y]);
            }
        }

        Texture2D texture = new Texture2D(size, size);

        texture.SetPixels(colorData);
        texture.Apply();

        return texture;
    }
    public static Texture2D DrawTexture(ChunkSettings chunkSettings, Func<float, Color> DrawPixel)
    {
        return DrawTexture(CalculateHeightData(chunkSettings), DrawPixel);
    }
    public Texture2D DrawTexture(Func<float, Color> DrawPixel)
    {
        return DrawTexture(heightData, DrawPixel);
    }

    public static Mesh CreateMesh(float[,] heightData, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int size = heightData.GetLength(0);

        // 计算网格精度
        int meshSamplificationIncrement = (int)Math.Pow(2, levelOfDetail);
        int verticesPerlin = (size - 1) / meshSamplificationIncrement + 1;

        // 网格数据
        Vector3[] vertices = new Vector3[size * size];
        Vector2[] uvs = new Vector2[size * size];
        int[] triangles = new int[(size - 1) * (size - 1) * 6];

        int verIndex = 0;
        int triIndex = 0;
        for (int y = 0; y < size; y += meshSamplificationIncrement)
        {
            for (int x = 0; x < size; x += meshSamplificationIncrement)
            {
                // 添加定点和UV
                vertices[verIndex] = new Vector3(x, heightCurve.Evaluate(heightData[x, y]) * heightMultiplier, -y);
                uvs[verIndex] = new Vector2((float)x / size, (float)y / size);

                // 添加三角面
                if (x < size - 1 && y < size - 1)
                {
                    triangles[triIndex++] = verIndex;
                    triangles[triIndex++] = verIndex + verticesPerlin + 1;
                    triangles[triIndex++] = verIndex + verticesPerlin;
                    triangles[triIndex++] = verIndex + verticesPerlin + 1;
                    triangles[triIndex++] = verIndex;
                    triangles[triIndex++] = verIndex + 1;
                }

                verIndex++;
            }
        }

        // 生成网格
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        return mesh;
    }
    public static Mesh CreateMesh(ChunkSettings chunkSettings)
    {
        return CreateMesh(CalculateHeightData(chunkSettings),
            chunkSettings.heightMultiplier, chunkSettings.heightCurve, chunkSettings.levelOfDetail);
    }
    public Mesh CreateMesh()
    {
        return CreateMesh(heightData,
            ChunkSettings.heightMultiplier, ChunkSettings.heightCurve, ChunkSettings.levelOfDetail);
    }

    public static void Get(ChunkSettings chunkSettings, ref MeshFilter filter, ref MeshRenderer renderer, TextureMode drawMode, Func<float, Color> DrawPixel = null)
    {
        float[,] heightData = CalculateHeightData(chunkSettings);

        filter.mesh = CreateMesh(heightData,
            chunkSettings.heightMultiplier, chunkSettings.heightCurve, chunkSettings.levelOfDetail);

        if (drawMode == TextureMode.HeightMap)
            renderer.sharedMaterial.mainTexture = DrawHeightMap(heightData);
        else if (drawMode == TextureMode.ColorMap)
            renderer.sharedMaterial.mainTexture = DrawColorMap(heightData, chunkSettings.regions);
        else
            renderer.sharedMaterial.mainTexture = DrawTexture(heightData, DrawPixel);
    }
    public static void Get(ChunkSettings chunkSettings, ref GameObject land, TextureMode drawMode, Func<float, Color> DrawPixel = null)
    {
        MeshFilter filter = land.GetComponent<MeshFilter>();
        MeshRenderer renderer = land.GetComponent<MeshRenderer>();

        Get(chunkSettings, ref filter, ref renderer, drawMode, DrawPixel);
    }
    public GameObject GetLand(TextureMode drawMode, Func<float, Color> DrawPixel = null)
    {
        GameObject chunk = GameObject.CreatePrimitive(PrimitiveType.Plane);
        MeshFilter filter = chunk.GetComponent<MeshFilter>();
        MeshRenderer renderer = chunk.GetComponent<MeshRenderer>();

        Get(ChunkSettings, ref filter, ref renderer, drawMode, DrawPixel);

        return chunk;
    }
}

public enum TextureMode
{
    HeightMap, ColorMap, CustomMap
}