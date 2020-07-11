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
    [Range(1, 10)]
    public int Octaves;
    [Range(0, 1)]
    public float Persistance;
    public float Lacunarity;
    public Vector2 Offset;
    [Header("Generate Objects")]
    public bool Trees;
    public enum TreeCount { None, Low, Medium, High };
    public TreeCount treeCount;
    Hashtable treeCountNumber = new Hashtable()
                {{ TreeCount.None, 0 }, { TreeCount.Low, 40 }, { TreeCount.Medium, 90 }, { TreeCount.High, 200 }};

    public GameObject[] TreePrefabs;
    public bool Others;
    public enum OthersCount { None, Low, Medium, High };
    public OthersCount otherCount;
    Hashtable otherCountNumber = new Hashtable()
                {{ OthersCount.None, 0 }, { OthersCount.Low, 10 }, { OthersCount.Medium, 22 }, { OthersCount.High, 50 }};
    public GameObject[] OtherPrefabs;
    [Header("Region Colors")]
    public TerrainType[] regions;


    public void GenerateMap()
    {
        clearPlacedObjects();
        float[,] noiseMap = generateNoiseMap(MapLength, MapWidth, Seed, Scale, Octaves, Persistance, Lacunarity, Offset);

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
            mapDisplay.drawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeight));
        }
        else if (drawMode == DrawMode.Object)
        {
            Texture2D texture = TextureGenerator.colorMapToTexture(noiseMap, MapWidth, MapLength, regions, MeshHeight);
            mapDisplay.drawTexture(texture);
            MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, MeshHeight);
            mapDisplay.drawMesh(meshData);
            placeObjects(meshData);
        }
    }

    public float[,] generateNoiseMap(int length, int width, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, length];

        System.Random ran = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = ran.Next(-1000000, 1000000) + offset.x;
            float offsetZ = ran.Next(-1000000, 1000000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetZ);
        }

        float halfWidth = width / 2;
        float halfLength = length / 2;

        if (scale < 0.0001f) { scale = 0.0001f; }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleZ = (z - halfLength) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, z] = noiseHeight;
            }
        }

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, z]);
            }
        }
        return noiseMap;
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

    public void clearPlacedObjects()
    {
        GameObject veg = GameObject.Find("Vegitation");
        int childCount = veg.transform.childCount;
        for (int m = 0; m < childCount; m++)
        {
            DestroyImmediate(veg.transform.GetChild(0).gameObject);
        }
        GameObject oth = GameObject.Find("OtherObjects");
        int childObCount = oth.transform.childCount;
        for (int m = 0; m < childObCount; m++)
        {
            DestroyImmediate(oth.transform.GetChild(0).gameObject);
        }
    }

    public void placeObjects(MeshData md)
    {
        System.Random rand = new System.Random();
        int m = 0, n = 0;
        while (m < (int)(treeCountNumber[treeCount]) || n < (int)(otherCountNumber[otherCount]))
        {
            int val = rand.Next(10000);
            float x = md.vertices[val].x;
            float z = md.vertices[val].z;
            float y = md.vertices[val].y;

            float originalX = 100 * (x - 0.5f);
            float originalZ = 100 * (z - 0.5f);

            if (y > 11 && m < (int)(treeCountNumber[treeCount]))
            {
                int vegIndex = rand.Next(TreePrefabs.Length);
                GameObject go = Instantiate(TreePrefabs[vegIndex], new Vector3(originalX, (50 * y), originalZ), Quaternion.identity);
                go.transform.localScale = go.transform.localScale * 10;
                go.transform.parent = GameObject.Find("Vegitation").transform;
                m++;
            }
            else if (y < 10 && n < (int)(otherCountNumber[otherCount]))
            {
                int otherIndex = rand.Next(OtherPrefabs.Length);
                GameObject go = Instantiate(OtherPrefabs[otherIndex], new Vector3(originalX, (50 * 10) - 10, originalZ), Quaternion.identity);
                go.transform.localScale = go.transform.localScale * 10;
                go.transform.parent = GameObject.Find("OtherObjects").transform;
                n++;
            }
        }
    }
}
