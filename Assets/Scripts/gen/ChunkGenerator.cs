using UnityEngine;
using System;

public static class ChunkGenerator
{
    public delegate Color ColorGenerator(float height);

    // 计算获得高度图
    public static float[,] CalculateHeightData(GenerateSetting chunkSettings)
    {
        float[,] heightData = new float[chunkSettings.size, chunkSettings.size];

        // 将每阶度噪声图进行随机化偏移
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

        // 转换为 0 到 1 的浮点数
        for (int y = 0; y < chunkSettings.size; y++)
        {
            for (int x = 0; x < chunkSettings.size; x++)
            {
                heightData[x, y] = Mathf.InverseLerp(minHeight, maxHeight, heightData[x, y]);
            }
        }

        return heightData;
    }

    // 给入生成数据，返回地块的黑白纹理图
    public static Texture2D DrawHeightMap(float[,] heightData)
    {
        return DrawTexture(heightData, (h) => new Color(h, h, h));
    }

    // 给入生成数据，返回地块区分高度的彩色高程图
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

    // 给入生成数据和自定义着色方案，返回自定义的纹理图
    public static Texture2D DrawTexture(float[,] heightData, ColorGenerator DrawPixel)
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

    // 根据生成数据生成地块模型
    public static Mesh CreateMesh(float[,] heightData, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int size = heightData.GetLength(0);
        int half = (size - 1) / 2;

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
                // 添加顶点和UV
                vertices[verIndex] = new Vector3(-half + x, heightCurve.Evaluate(heightData[x, y]) * heightMultiplier, half - y);
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
    public static Mesh CreateMesh(float[,] heightData, GenerateSetting chunkSettings)
    {
        return CreateMesh(heightData,
            chunkSettings.heightMultiplier, chunkSettings.heightCurve, chunkSettings.levelOfDetail);
    }

    // 生成地形
    public static void Get(float[,] heightData, GenerateSetting setting, GameObject chunk)
    {
        MeshFilter filter = chunk.GetComponent<MeshFilter>();
        MeshRenderer renderer = chunk.GetComponent<MeshRenderer>();

        // 纹理
        if (setting.drawMode == DrawMode.HeightMap)
            renderer.material.mainTexture = DrawHeightMap(heightData);
        else if (setting.drawMode == DrawMode.ColorMap)
            renderer.material.mainTexture = DrawColorMap(heightData, setting.regions);

        // 模型
        filter.mesh = CreateMesh(heightData, setting);
    }
}

public enum TextureMode
{
    HeightMap, ColorMap, CustomMap
}