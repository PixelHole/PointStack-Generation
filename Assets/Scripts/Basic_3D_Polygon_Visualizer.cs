using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_3D_Polygon_Visualizer : MonoBehaviour
{
    private PointStack_Generator worldGenerator;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    public bool UseHeightTexture;
    public bool UseSlopeTexture;
    public Gradient SlopeGradient;
    public int TextureUpscaling;
    public VisualizerColorLibrary ColorLibrary;
    void GetComponents()
    {
        worldGenerator = gameObject.GetComponent<PointStack_Generator>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }
    public void Visualise()
    {
        GetComponents();
        GenerateMesh();
    }
    void GenerateMesh()
    {
        int length = worldGenerator.length, width = worldGenerator.width;
        
        Vector3[] vertecies = new Vector3[width * length];
        Vector2[] uvs = new Vector2[vertecies.Length];
        int[] triangles = new int[((width - 1) * (length - 1)) * 6];
        
        Mesh mesh = new Mesh();

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float y = worldGenerator.GetWorldValueAt(x, z);
                vertecies[z * length + x] = new Vector3(x, y, z);
            }
        }
        
        for (int y = 0, i, t = 0; y < length - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                i = y * length + x;
                triangles[t + 0] = i;
                triangles[t + 1] = i + length;
                triangles[t + 2] = i + 1;
                triangles[t + 3] = i + length;
                triangles[t + 4] = i + length + 1;
                triangles[t + 5] = i + 1;
                t += 6;
            }
        }
        
        if (UseHeightTexture)
        {
            meshRenderer.sharedMaterial.mainTexture = GenerateHeightTexture(width, length, TextureUpscaling);
            for (int z = 0; z < length; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    uvs[z * length + x] = new Vector2((float)x / width, (float)z / length);
                }
            }
        }

        if (UseSlopeTexture)
        {
            meshRenderer.sharedMaterial.mainTexture = GenerateSlopeTexture(width, length, TextureUpscaling);
            for (int z = 0; z < length; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    uvs[z * length + x] = new Vector2((float)x / width, (float)z / length);
                }
            }
        }
        
        mesh.vertices = vertecies;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }

    private Texture2D GenerateSlopeTexture(int width, int length, int Upscaling)
    {
        Color[,] colors = new Color[width, length];
        float[,] diffs = new float[width, length];
        float GlobalMax = 0, GlobalMin = 0, MaxDiff;

        for (int y = 0; y < length - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                float min = worldGenerator.GetWorldValueAt(x, y)
                    , max = worldGenerator.GetWorldValueAt(x, y);
                
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        float height = worldGenerator.GetWorldValueAt(x + j, y + i);
                        if (height > max)
                        {
                            max = height;
                            if (max > GlobalMax)
                            {
                                GlobalMax = max;
                            }
                        }

                        if (height < min)
                        {
                            min = height;
                            if (min < GlobalMin)
                            {
                                GlobalMin = min;
                            }
                        }
                    }
                }
                diffs[x, y] = max - min;
            }
        }

        MaxDiff = GlobalMax - GlobalMin;
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[x, y] = SlopeGradient.Evaluate(diffs[x, y] / MaxDiff);
            }
        }
        
        Texture2D texture = CreateUpscaledTexture(width, length, Upscaling, colors);
        return texture;
    }
    private Texture2D GenerateHeightTexture(int width, int length, int Upscaling)
    {
        Color[,] colors = new Color[width, length];
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[x, y] = ColorLibrary.GetColorForHeight(worldGenerator.GetWorldValueAt(x, y));
            }
        }
        Texture2D texture = CreateUpscaledTexture(width, length, Upscaling, colors);
        return texture;
    }

    private Texture2D CreateUpscaledTexture(int width, int length,int Upscaling, Color[,] colors)
    {
        int ImageWidth = width * Upscaling, ImageLength = length * Upscaling;
        Texture2D texture = new Texture2D(ImageWidth, ImageLength);

        for (int Chunky = 0; Chunky < length; Chunky++)
        {
            for (int Chunkx = 0; Chunkx < width; Chunkx++)
            {
                int xposition = Chunkx * Upscaling, yposition = Chunky * Upscaling;
                
                Color HeightColor = colors[Chunkx, Chunky];
                
                for (int x = xposition; x < xposition + Upscaling; x++)
                {
                    for (int y = yposition; y < yposition + Upscaling; y++)
                    {
                        texture.SetPixel(x, y, HeightColor);
                    }
                }
            }
        }
        texture.Apply();
        return texture;
    }
}
