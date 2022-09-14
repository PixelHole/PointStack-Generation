using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Basic_3D_Visualizer : MonoBehaviour
{
    private PointStack_Generator WorldGenerator;
    public GameObject GroundTile;
    private List<GameObject> tiles = new List<GameObject>();
    public VisualizerColorLibrary ColorLibrary;
    private void Start()
    {
        GetGenerator();
    }
    void GetGenerator()
    {
        WorldGenerator = gameObject.GetComponent<PointStack_Generator>();
    }
    public void Visualize()
    {
        if (!WorldGenerator)
        {
            GetGenerator();
        }
        ClearCubes();
        for (int x = 0; x < WorldGenerator.width; x++)
        {
            for (int z = 0; z < WorldGenerator.length; z++)
            {
                float y = WorldGenerator.GetWorldValueAt(x,z);
                if (y != 0)
                {
                    Vector3 pos = new Vector3(x, y / 2, z);
                    tiles.Add(Instantiate(GroundTile, pos, Quaternion.identity, transform));
                    tiles[tiles.Count - 1].transform.localScale = new Vector3(1, y, 1);
                    tiles[tiles.Count - 1].GetComponent<MeshRenderer>().material.color = ColorLibrary.GetColorForHeight(y);
                }
            }
        }
    }

    public void ClearCubes()
    {
        foreach (var tile in tiles)
        {
            DestroyImmediate(tile);
        }
        tiles = new List<GameObject>();
    }
}
