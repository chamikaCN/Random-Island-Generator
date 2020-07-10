using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{

    public static Texture2D createTexture(Color[] colormap, int width, int length)
    {
        Texture2D texture = new Texture2D(width, length);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colormap);
        texture.Apply();
        return texture;
    }

    public static Texture2D heightMapToTexture(float[,] heightMap, int width, int length)
    {
        Texture2D texture = new Texture2D(width, length);

        Color[] colorMap = new Color[length * width];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[z * width + x] = Color.Lerp(Color.white, Color.black, heightMap[x, z]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        return createTexture(colorMap, width, length);
    }

    public static Texture2D colorMapToTexture(float[,] noiseMap, int width, int length, MapGenerator.TerrainType[] regions, int meshHeight)
    {
        Texture2D texture = new Texture2D(width, length);

        Color[] colorMap = new Color[length * width];

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float currentHeight = noiseMap[x, z];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= (regions[i].Height) * 20 / meshHeight)
                    {
                        if (regions[i].BlendColors && i < regions.Length - 1)
                        {
                            colorMap[z * width + x] = Color.Lerp(regions[i].Color, regions[i + 1].Color, noiseMap[x, z]);
                        }
                        else
                        {
                            colorMap[z * width + x] = regions[i].Color;
                        }
                        break;
                    }
                }
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        return createTexture(colorMap, width, length);
    }
}
