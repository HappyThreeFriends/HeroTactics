using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CheckPointPrefab;
    public GameObject PathPrefab;
    public event Action OnMapCreated;
    public IReadOnlyCollection<CheckPointController> CheckPoints => _checkpointsToNodes.Select(c => c.Key.GetComponent<CheckPointController>())
        .ToArray();

    private Dictionary<GameObject, Graph.Node> _checkpointsToNodes = new Dictionary<GameObject, Graph.Node>();
    private Graph _graph;

    void Start()
    {
        _graph = CreateTestMap();       
        DrawMap(_graph);
        OnMapCreated?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasDirectPath(CheckPointController from, CheckPointController to)
    {
        var fromNode = (Graph.Node)_checkpointsToNodes[from.gameObject];
        var toNode = (Graph.Node)_checkpointsToNodes[to.gameObject];

        return _graph.HasDirectPath(fromNode, toNode);
    }

    private Graph CreateTestMap()
    {
        var graph = new Graph();

        var baseNode = graph.AddNode(new Vector2(0, -5));
        var node21 = graph.AddNode(new Vector2(-2, -3));
        var node22 = graph.AddNode(new Vector2(0, -3));
        var node23 = graph.AddNode(new Vector2(2, -3));
        var node31 = graph.AddNode(new Vector2(-5, -1));
        var node32 = graph.AddNode(new Vector2(-3, -1));
        var node33 = graph.AddNode(new Vector2(-1, -1));
        var node34 = graph.AddNode(new Vector2(1, -1));
        var node35 = graph.AddNode(new Vector2(3, -1));
        var node36 = graph.AddNode(new Vector2(5, -1));

        graph.AddLink(baseNode, node21);
        graph.AddLink(baseNode, node22);
        graph.AddLink(baseNode, node23);
        
        //graph.AddLink(node1, node5);

        graph.AddLink(node21, node31);
        graph.AddLink(node21, node32);
        graph.AddLink(node21, node33);
        graph.AddLink(node22, node32);
        graph.AddLink(node22, node33);
        graph.AddLink(node22, node34);      
        graph.AddLink(node23, node34);
        graph.AddLink(node23, node35);
        graph.AddLink(node23, node36);
        //graph.AddLink(baseNode, node23);
        //graph.AddLink(baseNode, node31);

        return graph;
    }

    private void DrawMap(Graph graph)
    {
        foreach (var node in graph.Nodes)
        {
            var checkPoint = Instantiate(CheckPointPrefab, node.Coordinates, Quaternion.identity, this.transform);
            _checkpointsToNodes.Add(checkPoint, node);
        }

        foreach (var link in graph.Links)
        {
            var path = Instantiate(PathPrefab, link.From.Coordinates, CalculateRotation(link), this.transform);
            path.transform.localScale = new Vector3(CalculatePathLength(link), path.transform.localScale.y, path.transform.localScale.z);
        }
    }

    private float CalculatePathLength(Graph.Link link)
    {
        var distanceBetweenCheckpoints = link.To.Coordinates - link.From.Coordinates;
        return distanceBetweenCheckpoints.magnitude;
    }

    private Quaternion CalculateRotation(Graph.Link link)
    {
        var vec = link.To.Coordinates - link.From.Coordinates;
        var angle = Vector2.SignedAngle(Vector2.right, vec);       
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

public class Graph
{
    private List<Node> _nodes = new List<Node>();
    private List<Link> _links = new List<Link>();
    public IReadOnlyCollection<Node> Nodes => _nodes;
    public IReadOnlyCollection<Link> Links => _links;

    public bool HasDirectPath(Node from, Node to)
    {
        return _links.Any(l => l.From == from && l.To == to)
            || _links.Any(l => l.From == to && l.To == from);
    }

    public Node AddNode(Vector2 coordinates)
    {
        var node = new Node(coordinates);
        this._nodes.Add(node);

        return node;
    }

    public Link AddLink(Node from, Node to)
    {
        var link = new Link(from, to);
        this._links.Add(link);

        return link;
    }

    public class Node
    {
        public Node(Vector2 coordinates)
        {
            this.Coordinates = coordinates;
        }

        public Vector2 Coordinates { get; }
    }

    public class Link
    {
        public Link(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public Node From { get; }
        public Node To { get; }
    }
}