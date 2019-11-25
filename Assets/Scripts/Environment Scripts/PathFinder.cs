using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{

    public Transform startPosition, targetPosition;
    WorldGrid grid;
    public Transform floor;
    void Awake()
    {
        grid = GetComponent<WorldGrid>();
    }

    void Start()
    {
        FindPath(startPosition.position, targetPosition.position);
        CreateBlocks();
    }

    void FindPath(Vector3 startPos, Vector3 targetPositionPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetPositionNode = grid.NodeFromWorldPoint(targetPositionPos);
        

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

        grid.path = path;

    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        return Mathf.Abs(nodeA.gridX - nodeB.gridX) + Mathf.Abs(nodeA.gridY - nodeB.gridY);
    }

    void CreateBlocks()
    {
        Instantiate(floor, new Vector3(startPosition.position.x, startPosition.position.y, 0), Quaternion.identity);
        Instantiate(floor, new Vector3(targetPosition.position.x, targetPosition.position.y, 0), Quaternion.identity);
        foreach (var node in grid.path)
            Instantiate(floor, new Vector3(node.gridX, node.gridY, 0), Quaternion.identity);
    }
}