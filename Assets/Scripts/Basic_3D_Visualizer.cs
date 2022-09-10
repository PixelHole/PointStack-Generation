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
        WorldGenerator = GetComponent<SandMound_Generator>();
    }
    private void CreateCubes()
    {
        for (int x = 0; x < WorldGenerator.width; x++)
        {
            for (int z = 0; z < WorldGenerator.length; z++)
            {
                float y = WorldGenerator.GetWorldValueAt(x,z) / 2;
                Vector3 pos = new Vector3(x, y, z);
                tiles.Add(Instantiate(GroundTile, pos, Quaternion.identity, transform));
            }
        }
    }
}
