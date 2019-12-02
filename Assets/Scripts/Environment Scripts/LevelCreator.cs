using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelCreator : MonoBehaviour
{
    Vector2Int worldSize;
    [SerializeField]
    Transform lava = null; 
    [SerializeField]
    PlayerController player = null;
    [SerializeField]
    EnemyController enemy = null;
    [SerializeField]
    EndLevelPoint endLevelPoint = null;

    PathFinder pathFinder;
    WorldGrid grid;

    List<Tuple<Vector2Int, Vector2Int>> pointsPairs;
    List<Vector3> enemiesPositions;
    int minHeight;

    private void Awake()
    {
        pointsPairs = new List<Tuple<Vector2Int, Vector2Int>>();
        enemiesPositions = new List<Vector3>();

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
        //Количество пар
        int numberPairs = Random.Range(worldSize.x / 8, worldSize.x / 4);  
        //Количество пропастей
        int numberPrecipices = Random.Range(numberPairs / 3, numberPairs / 2);

        int step = worldSize.x / (numberPairs + numberPrecipices);
        //Ширина пропасти
        int precipiceSize = 2;

        //Начальная позиция ( 0, середина уровня +- 1/4 размера карты )
        Vector2Int point1 = new Vector2Int(0, Random.Range(worldSize.y / 2 - worldSize.y / 4, worldSize.y / 2 + worldSize.y / 4));
        Vector2Int point2 = new Vector2Int(point1.x + step, point1.y + Mathf.Clamp(Random.Range(-2, 2), 0, worldSize.y - 1));
                
        minHeight = point1.y;

        pointsPairs.Add(new Tuple<Vector2Int, Vector2Int>(point1, point2));

        for (int i = 1; i < numberPairs; i++)
        {
            //новая точка = (Предыдущая точка + ширина пропасти, генерируется высота от [-1 до 1) )
            point1 = new Vector2Int(pointsPairs[i - 1].Item2.x + precipiceSize, Mathf.Clamp(pointsPairs[i - 1].Item2.y + Random.Range(-1, 1), 0, worldSize.y - 1));
            point2 = new Vector2Int(Mathf.Clamp(point1.x + step, 0, worldSize.x), Mathf.Clamp(point1.y + Random.Range(-2, 2), 0, worldSize.y - 1));

            //Проверка на выход за размер карты
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

        List<Tuple<Vector2Int, Vector2Int>> availablePairs = new List<Tuple<Vector2Int, Vector2Int>>();
        availablePairs.AddRange(pointsPairs);
        availablePairs.RemoveAt(0);

        for (int i = 0; i < numberEnemies; i++)
        {
            int indexPair = Random.Range(0, availablePairs.Count);
            int indexPath = pathFinder.Grid.path.FindIndex(x => x.gridX == availablePairs[indexPair].Item1.x);
            for (int j = availablePairs[indexPair].Item1.x; j < availablePairs[indexPair].Item2.x; j++)
            {
                if (pathFinder.Grid.path[indexPath].gridY == pathFinder.Grid.path[indexPath + 1].gridY)
                {
                    enemiesPositions.Add(new Vector3(pathFinder.Grid.path[indexPath + 1].gridX, pathFinder.Grid.path[indexPath + 1].gridY + 1, 1));
                    Instantiate(enemy, new Vector3(pathFinder.Grid.path[indexPath + 1].gridX, pathFinder.Grid.path[indexPath + 1].gridY + 1, 1), Quaternion.identity);
                    break;
                }
                indexPath++;
            }
            availablePairs.RemoveAt(indexPair);
        }

        //Debug.LogFormat($"{enemiesPositions.Count} {pointsPairs.Count}");            
    }

    public void CreateBlocks()
    {        
        foreach (var node in pathFinder.Grid.path)
        {
            Instantiate(pathFinder.Floor, new Vector3(node.gridX, node.gridY, 1), Quaternion.identity);
        }

        lava.transform.localScale = new Vector3(worldSize.x + worldSize.x / 2, 1, 1);
        Instantiate(lava, new Vector3(worldSize.x / 2, minHeight - 1, 1), Quaternion.identity);

        endLevelPoint.transform.position = new Vector3(pathFinder.Grid.path[pathFinder.Grid.path.Count - 1].gridX, pathFinder.Grid.path[pathFinder.Grid.path.Count - 1].gridY + 2, 1);
    }

    public void RestartLevel()
    {
        int i = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.transform.position = new Vector3(enemiesPositions[i].x, enemiesPositions[i++].y, 1);
        }
        
        player.ChangeHealth(player.MaxHealth);
        player.transform.position = new Vector3(pointsPairs[0].Item1.x, pointsPairs[0].Item1.y + 1, 1);
    }

    

}




