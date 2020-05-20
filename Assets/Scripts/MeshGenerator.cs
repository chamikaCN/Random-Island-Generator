using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static void GenerateTerrainMesh(float[,] heightMap){
        int width = heightMap.GetLength(0);
        int length = heightMap.GetLength(1);

         
    }
}

public class MeshData{
    public Vector3[] vertices;
    public int[] triangles;

    int triangleIndex;

    public MeshData(int width, int length){
        vertices = new Vector3[width * length];
        triangles = new int[(width - 1) * (length - 1) * 6];
    }

    public void addTriangles(int a, int b, int c){
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }
}
