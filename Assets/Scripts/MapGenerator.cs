using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColorMap, Mesh, Object };
    public DrawMode drawMode;
    [Range(18, 28)]
    public int MeshHeight;
    public int MapWidth;
    public int MapLength;
    public int Seed;
    public float Scale;
    public bool AutoUpdate;
    public bool FallOff;
    [Range(1, 10)]
    public int FallOffSpread;
    [Range(0, 10)]
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;
    public Vector2 Offset;
    [Header("Generate Objects")]
    public bool Trees;
    public bool Rocks;
    [Header("Region Colors")]
    public TerrainType[] regions;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.generateNoiseMap(MapLength, MapWidth, Seed, Scale, Octaves, Persistance, Lacunarity, Offset);

        if (FallOff) { AddFalloffEfect(noiseMap); }
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
        {
            mapDisplay.drawTexture(TextureGenerator.heightMapToTexture(noiseMap, MapWidth, MapLength));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            mapDisplay.drawTexture(TextureGenerator.colorMapToTexture(noiseMap, MapWidth, MapLength, regions, MeshHeight));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            Texture2D texture = TextureGenerator.colorMapToTexture(noiseMap, MapWidth, MapLength, regions, MeshHeight);
            mapDisplay.drawTexture(texture);
            mapDisplay.drawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeight), texture);
        }
        else if (drawMode == DrawMode.Object)
        {
            Texture2D texture = TextureGenerator.colorMapToTexture(noiseMap, MapWidth, MapLength, regions, MeshHeight);
            mapDisplay.drawTexture(texture);
            mapDisplay.drawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeight), texture);

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

    public void AddFalloffEfect(float[,] nMap)
    {
        int length = nMap.GetLength(0);
        int width = nMap.GetLength(1);
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float i = 1 - ((2 * x) / (float)width);
                float j = 1 - ((2 * z) / (float)length);

                float val = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j));
                float FOval = getFalloffValue(val);
                nMap[x, z] = Mathf.Clamp01(nMap[x, z] - FOval);
            }
        }

    }

    public float getFalloffValue(float value)
    {
        float a = 3;
        float b = FallOffSpread;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
