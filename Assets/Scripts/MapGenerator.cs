using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap };
    public DrawMode drawMode;
    public int MapWidth;
    public int MapLength;
    public int Seed;
    public float Scale;
    public bool AutoUpdate;
    [Range(0, 10)]
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;
    public Vector2 Offset;
    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.generateNoiseMap(MapLength, MapWidth, Seed, Scale, Octaves, Persistance, Lacunarity, Offset);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.drawTexture(TextureGenerator.heightMapToTexture(noiseMap,MapWidth,MapLength));
        }else if(drawMode == DrawMode.ColorMap){
            Color[] colorMap = new Color[MapWidth * MapLength];
            for (int z = 0; z < MapLength; z++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    float currentHeight = noiseMap[x, z];
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (currentHeight <= regions[i].Height)
                        {
                            if(regions[i].BlendColors && i<regions.Length-1){
                                colorMap[z * MapWidth + x] = Color.Lerp(regions[i].Color, regions[i+1].Color, noiseMap[x, z]);
                            }
                            else
                            {
                                colorMap[z * MapWidth + x] = regions[i].Color;
                            }
                            break;
                        }
                    }
                }
            }
            mapDisplay.drawTexture(TextureGenerator.colorMapToTexture(colorMap, MapWidth, MapLength));
        }
    }

    private void OnValidate()
    {
        if (MapWidth < 1) { MapWidth = 1; }
        if (MapLength < 1) { MapLength = 1; }
        if (Lacunarity < 1) { Lacunarity = 1; }
        if (Octaves < 0) { Octaves = 0; }
        if (MapWidth < 1) { MapWidth = 1; }
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string Name;
        public float Height;
        public Color Color;
        public bool BlendColors;

    }
}
