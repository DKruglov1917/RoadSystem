using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour                 //using A* algorithm
{
    public static List<Vector2> PathToTarget;
    public List<Node> CheckedNodes = new List<Node>();
    public List<Node> FreeNodes = new List<Node>();
    public List<Node> WaitingNodes = new List<Node>();
    public GameObject Target;

    public static GameObject roadPrefab;
    private bool bActive;

    private void Awake()
    {
        bActive = true;

        BuildingsGrid.onFinish += InstantiateRoad;
        roadPrefab = GameObject.Find("Road");
    }

    public List<Vector2> GetPath(Vector2 target)
    {
        PathToTarget = new List<Vector2>();
        CheckedNodes = new List<Node>();
        WaitingNodes = new List<Node>();

        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));

        if (StartPosition == TargetPosition) return PathToTarget;

        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(GetNeighbourNodes(startNode));
        while (WaitingNodes.Count > 0)
        {
            Node nodeToCheck = WaitingNodes.Where(x => x.F == WaitingNodes.Min(y => y.F)).FirstOrDefault();

            if (nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            WaitingNodes.Remove(nodeToCheck);
            if (!CheckedNodes.Where(x => x.Position == nodeToCheck.Position).Any())
            {
                CheckedNodes.Add(nodeToCheck);
                WaitingNodes.AddRange(GetNeighbourNodes(nodeToCheck));                
            }
        }
        FreeNodes = CheckedNodes;

        return PathToTarget;
    }

    public List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;

        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.Position.x, currentNode.Position.y));
            currentNode = currentNode.PreviousNode;
        }

        return path;
    }

    public void InstantiateRoad()
    {
        if (bActive)
        {
            PathToTarget = GetPath(Target.transform.position);
            bActive = false;
        }
    }

    List<Node> GetNeighbourNodes(Node node)
    {
        var Neighbours = new List<Node>();

        Neighbours.Add(new Node(node.G + 1, new Vector2(
            node.Position.x - 1, node.Position.y),
            node.TargetPosition,
            node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
            node.Position.x + 1, node.Position.y),
            node.TargetPosition,
            node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
            node.Position.x, node.Position.y - 1),
            node.TargetPosition,
            node));
        Neighbours.Add(new Node(node.G + 1, new Vector2(
            node.Position.x, node.Position.y + 1),
            node.TargetPosition,
            node));
        return Neighbours;
    }    

}

public class Node
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public Node PreviousNode;
    public int F; // F=G+H
    public int G; // distance from start to node
    public int H; // distance from node to target

    public Node(int g, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = G + H;
    }
}
