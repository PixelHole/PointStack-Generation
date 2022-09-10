using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_3D_Visualizer : MonoBehaviour
{
    private SandMound_Generator WorldGenerator;
    public GameObject GroundTile;
    private List<GameObject> tiles = new List<GameObject>();
    private void Start()
    {
        GetGenerator();
    }
    void GetGenerator()
    {
        WorldGenerator = gameObject.GetComponent<SandMound_Generator>();
    }
    public void CreateCubes()
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
