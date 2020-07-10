using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPlacer
{
    static float[,] noiseMap;
    static int mapSize;
    static System.Random random;
    public static GameObject tree1, tree2, tree3, tree4;
    public static void placeInSand(int count)
    {
        int j = 0;
        random = new System.Random();
        while (j < count)
        {
            int m = random.Next(0, mapSize);
            int n = random.Next(0, mapSize);
            if (noiseMap[m, n] > 0.5 && noiseMap[m, n] < 0.7)
            {
                Debug.Log(noiseMap[m, n]);
                j++;
            }
            else
            {
                continue;
            }

        }
    }

    public static void setNoiseMap(float[,] map, int size)
    {
        noiseMap = map;
        mapSize = size;
    }
}
