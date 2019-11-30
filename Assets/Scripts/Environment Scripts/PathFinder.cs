using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    WorldGrid grid;
    public WorldGrid Grid { get; set; }
    [SerializeField]
    Transform floor;
    public Transform Floor { get => floor; set => floor = value; }
    void Awake()
    {
        grid = GetComponent<WorldGrid>();
    }

    public void FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        //Создается 2 списка вершин.
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetPositionNode = grid.NodeFromWorldPoint(targetPos);
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        //В ожидающие добавляется точка старта, список рассмотренных пока пуст.
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                //Из списка точек на рассмотрение выбирается точка с наименьшим F. Обозначим ее X
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost && openSet[i].hCost < node.hCost)
                {
                    
                        node = openSet[i];
                }
            }

            //Переносим X из списка ожидающих рассмотрения в список уже рассмотренных.
            openSet.Remove(node);
            closedSet.Add(node);

            //Если X — цель, то мы нашли маршрут
            if (node == targetPositionNode)
            {
                RetracePath(startNode, targetPositionNode);
                return;
            }

            //Для каждой из точек, соседних для X (обозначим эту соседнюю точку Y), делаем следующее
            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                //Если Y уже находится в рассмотренных — пропускаем ее.
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                //Если же Y в списке на рассмотрение — проверяем, если X.G + расстояние от X до Y < Y.G
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    //Y.G на X.G + расстояние от X до Y
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetPositionNode);
                    //точку, из которой пришли в Y на X
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
}