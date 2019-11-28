using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGrid : MonoBehaviour
{
    
    [SerializeField]
    Vector2Int worldSize;
    public Vector2Int WorldSize { get => worldSize; 
                                    set { worldSize = value;
                                         CreateGrid();}}

    public Node[,] grid;
    
    public List<Node> path;
    
    void Awake()
    {
        path = new List<Node>();
        CreateGrid();
    }

    public void CreateGrid()
    {
        grid = new Node[worldSize.x, worldSize.y];
        
        for (int x = 0; x < worldSize.x; x++)
        {
            for (int y = 0; y < worldSize.y; y++)
            {
                bool walkable = true;
                Vector2Int worldPoint = new Vector2Int(x, y);
                

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < worldSize.x && checkY >= 0 && checkY < worldSize.y)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector2Int worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);

        return grid[x, y];
    } 
}