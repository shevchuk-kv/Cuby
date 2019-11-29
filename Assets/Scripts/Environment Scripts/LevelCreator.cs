using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : MonoBehaviour
{
    Vector2Int worldSize;
    [SerializeField]
    Transform lava; 
    [SerializeField]
    PlayerController player;
    [SerializeField]
    EnemyController enemy;

    PathFinder pathFinder;
    WorldGrid grid;
    List<Tuple<Vector2Int, Vector2Int>> pointsPairs;
    List<Vector2Int> enemiesPositions;
    int minHeight;

    private void Awake()
    {
        pointsPairs = new List<Tuple<Vector2Int, Vector2Int>>();
        enemiesPositions = new List<Vector2Int>();

        pathFinder = GetComponent<PathFinder>();
        grid = GetComponent<WorldGrid>();
        
        pathFinder.Grid = grid;
        worldSize = pathFinder.Grid.WorldSize;

        GenerateLevel();
        CreateBlocks();
        CreateEntities();
    }


    public void GenerateLevel()
    {
        int numberPairs = Random.Range(worldSize.x / 4, worldSize.x / 2);
        int numberPrecipices = Random.Range(numberPairs / 3, numberPairs / 2);
        int step = worldSize.x / (numberPairs + numberPrecipices);

        Vector2Int point1 = new Vector2Int(0, Random.Range(worldSize.y / 2 - worldSize.y / 4, worldSize.y / 2 + worldSize.y / 4));
        Vector2Int point2 = new Vector2Int(point1.x + step, point1.y + Mathf.Clamp(Random.Range(-3, 2), 0, worldSize.y - 1));

        minHeight = point1.y;

        pointsPairs.Add(new Tuple<Vector2Int, Vector2Int>(point1, point2));

        for (int i = 1; i < numberPairs; i++)
        {
            point1 = new Vector2Int(pointsPairs[i - 1].Item2.x + 2, Mathf.Clamp(pointsPairs[i - 1].Item2.y + Random.Range(-1, 1), 0, worldSize.y - 1));
            point2 = new Vector2Int(Mathf.Clamp(point1.x + step, 0, worldSize.x), Mathf.Clamp(point1.y + Random.Range(-3, 2), 0, worldSize.y - 1));

            if (point1.x >= worldSize.x || point2.x >= worldSize.x)
                break;
            if (minHeight > point1.y || minHeight > point2.y)
                minHeight = Math.Min(point1.y, point2.y);

            pointsPairs.Add(new Tuple<Vector2Int, Vector2Int>(point1, point2));
        }

        foreach(var pair in pointsPairs)
        {
            //Debug.LogFormat($"{pair.Item1} {pair.Item2} {pointsPairs.Count}");
            pathFinder.FindPath(pair.Item1, pair.Item2);
        }                
    }

    public void CreateEntities()
    {
        player.transform.position = new Vector3(pointsPairs[0].Item1.x, pointsPairs[0].Item1.y + 1, 1);

        int numberEnemies = Mathf.Clamp(Random.Range(worldSize.x / 20, worldSize.x / 8), 0, pointsPairs.Count);        

        for (int i = 0; i < numberEnemies; i++)
        {
            //int index = Random.Range(5, pathFinder.Grid.path[pathFinder.Grid.path.Count - 1].gridX);
            //enemiesPositions.Add(new Vector2Int(pathFinder.Grid.path[index].gridX, pathFinder.Grid.path[index].gridY + 1));
            //Instantiate(enemy, new Vector3(pathFinder.Grid.path[index].gridX, pathFinder.Grid.path[index].gridY + 1, 1), Quaternion.identity);

            int index = Random.Range(1, pointsPairs.Count - 1);
            enemiesPositions.Add(new Vector2Int(pointsPairs[index].Item1.x, pointsPairs[index].Item1.y + 1));
            Instantiate(enemy, new Vector3(pointsPairs[index].Item1.x, pointsPairs[index].Item1.y + 1, 1), Quaternion.identity);
        }
    }

    public void CreateBlocks()
    {        
        foreach (var node in pathFinder.Grid.path)
        {
            Instantiate(pathFinder.Floor, new Vector3(node.gridX, node.gridY, 1), Quaternion.identity);
        }

        lava.transform.localScale = new Vector3(worldSize.x + worldSize.x / 2, 1, 1);
        Instantiate(lava, new Vector3(worldSize.x / 2, minHeight - 1, 1), Quaternion.identity);        
    }

}




