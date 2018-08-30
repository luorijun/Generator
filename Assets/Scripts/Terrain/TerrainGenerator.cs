using UnityEngine;
using System;

public static class TerrainGenerator
{

    public static float[,] GenerateHeightData(int seed, int width, int height, float scale, float persistance, float lacunarity, int octaves, Vector2 offset)
    {
        float[,] heightData = new float[width, height];

        System.Random random = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;

                float totalHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float xCoord = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float yCoord = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float currentHeight = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
                    totalHeight += currentHeight * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                maxHeight = Mathf.Max(maxHeight, totalHeight);
                minHeight = Mathf.Min(minHeight, totalHeight);

                heightData[x, y] = totalHeight;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                heightData[x, y] = Mathf.InverseLerp(minHeight, maxHeight, heightData[x, y]);
            }
        }

        return heightData;
    }

    public static Texture2D GetHeightMap(float[,] heightData)
    {
        return RenderingTexture(heightData, (h) =>
        {
            return new Color(h, h, h);
        });
    }

    public static Texture2D GetColorMap(float[,] heightData, RegionData[] regions)
    {
        return RenderingTexture(heightData, (h) =>
        {
            for (int i = 0; i < regions.Length; i++)
            {
                if (h <= regions[i].height)
                    return regions[i].color;
            }
            return Color.white;
        });
    }

    private delegate Color RenderingMode(float height);
    private static Texture2D RenderingTexture(float[,] heightData, RenderingMode rendering)
    {
        int width = heightData.GetLength(0);
        int height = heightData.GetLength(1);

        Color[] colorData = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorData[y * width + x] = rendering(heightData[x, y]);
            }
        }

        Texture2D texture = new Texture2D(width, height);

        //texture.filterMode = FilterMode.Point;
        //texture.wrapMode = TextureWrapMode.Clamp;

        texture.SetPixels(colorData);
        texture.Apply();

        return texture;
    }

    public static Mesh GetTerrainMesh(float[,] heightData, float heightMultiplier, AnimationCurve heightCurve)
    {
        int width = heightData.GetLength(0);
        int height = heightData.GetLength(1);

        MeshData meshData = new MeshData(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = meshData.AppendVertice(new Vector3(x, heightCurve.Evaluate(heightData[x, y]) * heightMultiplier, -y));
                meshData.AppendUV(new Vector2((float)x / width, (float)y / height));
                if (x == width - 1 || y == height - 1) continue;

                meshData.AppendTriangles(index, index + width + 1, index + width);
                meshData.AppendTriangles(index + width + 1, index, index + 1);
            }
        }

        return meshData.CreateMeshFromData();
    }

    private class MeshData
    {
        private Vector3[] vertices;
        private Vector2[] uvs;
        private int[] triangles;


        private int verIndex;
        private int uvIndex;
        private int triIndex;

        public MeshData(int width, int height)
        {
            vertices = new Vector3[width * height];
            uvs = new Vector2[width * height];
            triangles = new int[(width - 1) * (height - 1) * 6];

            verIndex = 0;
            triIndex = 0;
        }

        public int AppendVertice(Vector3 vector)
        {
            vertices[verIndex++] = vector;
            return verIndex - 1;
        }

        public void AppendUV(Vector2 vector)
        {
            uvs[uvIndex++] = vector;
        }

        public void AppendTriangles(int a, int b, int c)
        {
            triangles[triIndex++] = a;
            triangles[triIndex++] = b;
            triangles[triIndex++] = c;
        }

        public Mesh CreateMeshFromData()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            return mesh;
        }
    }
}

[Serializable]
public struct RegionData
{
    public string name;
    public float height;
    public Color color;
}