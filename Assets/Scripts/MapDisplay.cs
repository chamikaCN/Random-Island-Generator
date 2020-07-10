using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void drawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(-3*texture.width, 1, 3*texture.height);
    }

    public void drawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
    }
}
