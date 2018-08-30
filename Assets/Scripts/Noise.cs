using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
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
}
