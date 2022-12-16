using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day16
{
  internal class Graph
  {
    private readonly Regex LineParser = new("Valve (\\w+) has flow rate=(\\d+); tunnels? leads? to valves? (.+)");

    public Dictionary<string, Node> Nodes { get; } = new();

    public void AddNode(string line)
    {
      Node node = CreateNodeFromLine(line);
      Nodes.Add(node.Name, node);
    }

    private Node CreateNodeFromLine(string line)
    {
      Match m = LineParser.Match(line);
      if (!m.Success)
      {
        throw new ArgumentException($"Line could not be parsed: {line}");
      }
      string nodeName = m.Groups[1].Value;
      int flowRate = int.Parse(m.Groups[2].Value);
      string[] connections = m.Groups[3].Value.Split(", ");
      Node node = new(nodeName, flowRate, connections);
      return node;
    }

    public ShortestPath CalculateShortestDistance(Node startNode, Node endNode)
    {
      ISet<Node> Unvisited = new HashSet<Node>();
      IDictionary<Node, Node> BestPrevious = new Dictionary<Node, Node>();
      IDictionary<Node, int> BestStartDistance = new Dictionary<Node, int>();
      foreach (Node node in Nodes.Values)
      {
        BestStartDistance[node] = int.MaxValue;
        Unvisited.Add(node);
      }
      BestStartDistance[startNode] = 0;
      Node? curNode = startNode;

      while (Unvisited.Contains(endNode))
      {
        List<Node> neighbors = curNode.TunnelsTo.Select(s => Nodes[s]).ToList();
        foreach (Node neighbor in neighbors.Where(n => Unvisited.Contains(n)))
        {
          int newDistance = BestStartDistance[curNode] + 1;
          if (newDistance < BestStartDistance[neighbor])
          {
            BestStartDistance[neighbor] = newDistance;
            BestPrevious[neighbor] = curNode;
          }
        }
        Unvisited.Remove(curNode);
        if (Unvisited.Count == 0)
        {
          break;
        }
        curNode = Unvisited.OrderBy(n => BestStartDistance[n]).First();
        if (BestStartDistance[curNode] == int.MaxValue)
        {
          break;
        }
      }

      List<Node> path = new();
      curNode = endNode;
      while (curNode != null)
      {
        path.Add(curNode);
        BestPrevious.TryGetValue(curNode, out curNode);
      }
      path.Reverse();

      ShortestPath shortestPath = new()
      {
        Start = startNode.Name,
        End = endNode.Name
      };
      shortestPath.PathNodes.AddRange(path);

      return shortestPath;
    }
  }
}