using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, int MeshHeight)
    {
        int width = heightMap.GetLength(0);
        int length = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (length - 1) / 2f;

        MeshData meshData = new MeshData(width, length);
        int vertexIndex = 0;

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, MeshHeight * heightMap[x, z], topLeftZ - z);
                meshData.UVs[vertexIndex] = new Vector2(x / (float)width, z / (float)length);
                if (x < width - 1 && z < length - 1)
                {
                    meshData.addTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.addTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] UVs;

    int triangleIndex = 0;

    public MeshData(int width, int length)
    {
        vertices = new Vector3[width * length];
        UVs = new Vector2[width * length];
        triangles = new int[(width - 1) * (length - 1) * 6];
    }

    public void addTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
