using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{

    //public Vector2Int startPosition, targetPosition;
    WorldGrid grid;
    public WorldGrid Grid { get; set; }
    public Transform floor;
    public Transform Floor { get => floor; set => floor = value; }
    void Awake()
    {
        grid = GetComponent<WorldGrid>();
    }

    void Start()
    {
        
        //FindPath(startPosition, targetPosition);
                
        //CreateBlocks();
    }

    public void FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        //Debug.LogFormat($"{startPos.x} {targetPos.x}");
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetPositionNode = grid.NodeFromWorldPoint(targetPos);
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost && openSet[i].hCost < node.hCost)
                {
                    
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetPositionNode)
            {
                
                RetracePath(startNode, targetPositionNode);
                //Debug.LogFormat($"{grid.path.Count}");
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetPositionNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path.Add(startNode);        
        grid.path.AddRange(path);
        grid.path.Add(endNode);
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        return Mathf.Abs(nodeA.gridX - nodeB.gridX) + Mathf.Abs(nodeA.gridY - nodeB.gridY);
    }

    public void CreateBlocks()
    {
        foreach (var node in grid.path)
            Instantiate(floor, new Vector3(node.gridX, node.gridY, 0), Quaternion.identity);
    }
}