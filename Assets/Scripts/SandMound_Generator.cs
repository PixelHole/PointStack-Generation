using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SandMound_Generator : MonoBehaviour
{
    //Input Variables
    public int width, length; // width = x and length = y in array;
    //Test Input variables
    [Header("Walker settings")] 
    public Vector2Int position;
    public bool Turn;
    public float TurnChance;
    public int Levels, lifetime;
    //Data
    private float[,] world;
    private void Start()
    {
        InitialiseWorld();
    }
    private void InitialiseWorld()
    {
        world = new float[width, length];
    }
    private void GenerateWorld()
    {
        for (int i = 0; i < Levels; i++)
        {
            walker walker = new walker(position, Turn, i, TurnChance, lifetime);
            while (lifetime > 0)
            {
                if (world[walker.Position.x, walker.Position.y] < walker.Level)
                {
                    world[walker.Position.x, walker.Position.y]++;
                }
                walker.Step();
            }
        }
    }

    public float GetWorldValueAt(int x, int y)
    {
        return world[x, y];
    }
}

class walker
{
    public Vector2Int Position, Direction;
    public float Turn_Chance;
    public int Level, Lifetime;
    public bool Turn, OverLayTile, Manual_Level;

    public walker(Vector2Int position, bool turn, int level,float TurnChance, int lifetime)
    {
        Position = position;
        Turn = turn;
        Level = level;
        Turn_Chance = TurnChance;
        this.Lifetime = lifetime;
    }
    public void Step()
    {
        if (Random.Range(0,1) < Turn_Chance)
        {
            List<Vector2Int> directions = new List<Vector2Int>()
            {
                Vector2Int.down,
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up
            };
            if (Turn)
            {
                directions.Remove(-Direction);
            }
            Direction = directions[Random.Range(0, directions.Count)];
        }
        Position += Direction;
        Lifetime--;
    }
}