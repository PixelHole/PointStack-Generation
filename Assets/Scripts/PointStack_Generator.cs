using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PointStack_Generator : MonoBehaviour
{
    //Input Variables
    public int width, length; // width = x and length = y in array;
    
    //Test Input variables
    [Header("Walker settings")] 
    public Vector2Int position;
    public bool Turn;
    public float TurnChance, value;
    public int Levels, lifetime, Count;
    
    //Data
    private float[,] world;

    //Events
    public UnityEvent WorldGenerationFinished = new UnityEvent();
    
    private void Start()
    {
        Create();
    }
    public void Create()
    {
        InitialiseWorld();
        GenerateWorld();
    }
    private void InitialiseWorld()
    {
        world = new float[width, length];
    }
    private void ClearWorld()
    {
        
    }
    private void GenerateWorld()
    {
        Vector2Int worldsize = new Vector2Int(width, length);
        for (int i = 0; i < Levels; i++)
        {
            List<walker> walkers = new List<walker>();
            for (int j = 0; j < Count; j++)
            {
                walkers.Add(new walker(position, Turn, i + 1, TurnChance, lifetime, worldsize, value));
            }
            while (walkers.Count > 0)
            {
                foreach (var walker in walkers.ToArray())
                {
                    if (walker.Lifetime > 0)
                    {
                        if (world[walker.Position.x, walker.Position.y] < walker.Target_Level)
                        {
                            world[walker.Position.x, walker.Position.y] += walker.Value;
                            walker.ReduceLifetime(1);
                        }
                        walker.Step();
                    }
                    else
                    {
                        walkers.Remove(walker);
                    }
                }
            }
        }
        WorldGenerationFinished.Invoke();
    }
    public float GetWorldValueAt(int x, int y)
    {
        return world[x, y];
    }
}

class walker
{
    public Vector2Int Position, Direction, WorldSize;
    public float Turn_Chance, Value;
    public int Target_Level, Lifetime;
    public bool Turn, OverLayTile, Manual_Level;

    public walker(Vector2Int position, bool turn, int targetLevel,float TurnChance, int lifetime, Vector2Int worldSize, float value)
    {
        Position = position;
        Turn = turn;
        Target_Level = targetLevel;
        Turn_Chance = TurnChance;
        Lifetime = lifetime;
        WorldSize = worldSize;
        Value = value;
    }
    public void Step()
    {
        if (Random.Range(0,100) < Turn_Chance)
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
                directions.Remove(Direction);
            }
            Direction = directions[Random.Range(0, directions.Count)];
        }

        if (IsPositionInBounds(Position + Direction))
        {
            Position += Direction;
        }
    }

    public void ReduceLifetime(int value)
    {
        Lifetime -= value;
    }
    public bool IsPositionInBounds(Vector2Int pos)
    {
        return pos.x > 0 & pos.y > 0 & pos.x < WorldSize.x & pos.y < WorldSize.y;
    }
}