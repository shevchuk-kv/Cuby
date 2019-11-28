using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : MonoBehaviour
{
    [SerializeField]
    Vector2Int worldSize;
    [SerializeField]
    Transform lava;
    [SerializeField]
    Transform floor;    
    [SerializeField]
    PlayerController player;

    PathFinder pathFinder;
    WorldGrid grid;
    List<Tuple<Vector2Int, Vector2Int>> pointsPairs;

    private void Awake()
    {
        //floor = GetComponent<Transform>();
        pointsPairs = new List<Tuple<Vector2Int, Vector2Int>>();
        pathFinder = GetComponent<PathFinder>();
        grid = GetComponent<WorldGrid>();
        grid.WorldSize = worldSize;
        pathFinder.Grid = grid;
        pathFinder.Floor = floor;
          
        GenerateLevel();
    }


    public void GenerateLevel()
    {
        int numberPairs = Random.Range(worldSize.x / 4, worldSize.x / 2);
        int numberPrecipices = Random.Range(numberPairs / 3, numberPairs / 2);
        int step = worldSize.x / (numberPairs + numberPrecipices);

        Vector2Int point1 = new Vector2Int(0, Random.Range(worldSize.y / 2 - worldSize.y / 4, worldSize.y / 2 + worldSize.y / 4));
        Vector2Int point2 = new Vector2Int(point1.x + step, point1.y + Mathf.Clamp(Random.Range(-3, 3), 0, worldSize.y - 1));

        pointsPairs.Add(new Tuple<Vector2Int, Vector2Int>(point1, point2));

        for (int i = 1; i < numberPairs; i++)
        {
            if (pointsPairs[i - 1].Item2.x + step >= worldSize.x || pointsPairs[i - 1].Item1.x + step >= worldSize.x)
                break;            

            point1 = new Vector2Int(pointsPairs[i - 1].Item2.x + 2, Mathf.Clamp(pointsPairs[i - 1].Item2.y + Random.Range(-1, 1), 0, worldSize.y - 1));
            point2 = new Vector2Int(Mathf.Clamp(point1.x + step, 0, worldSize.x), Mathf.Clamp(point1.y + Random.Range(-3, 3), 0, worldSize.y - 1));

            if (point1.x >= worldSize.x || point2.x >= worldSize.x)
                break;

            pointsPairs.Add(new Tuple<Vector2Int, Vector2Int>(point1, point2));
        }

        foreach(var pair in pointsPairs)
        {
            Debug.LogFormat($"{pair.Item1} {pair.Item2} {pointsPairs.Count}");
            pathFinder.FindPath(pair.Item1, pair.Item2);
        }

        pathFinder.CreateBlocks();
        player.transform.position = new Vector3(pointsPairs[0].Item1.x, pointsPairs[0].Item1.y + 1, 1);
    }

    
}




