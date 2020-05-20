using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator{

    public static Texture2D colorMapToTexture(Color[] colormap, int width, int length){
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

        return colorMapToTexture(colorMap, width, length);
    }
}
