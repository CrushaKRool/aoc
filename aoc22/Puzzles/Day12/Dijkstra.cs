using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day12
{
  internal sealed class Dijkstra
  {
    public Node[][] Grid { get; }
    private readonly int sizeX;
    private readonly int sizeY;

    public Dijkstra(string input)
    {
      Grid = ParseGrid(input);
      sizeY = Grid.Length;
      sizeX = Grid[0].Length;
    }

    public IEnumerable<Node> GetAllNodesWithElevation(char elevation)
    {
      for (int x = 0; x < sizeX; x++)
      {
        for (int y = 0; y < sizeY; y++)
        {
          Node curNode = Grid[y][x];
          if (curNode.Elevation == elevation)
          {
            yield return curNode;
          }
        }
      }
    }

    public List<Node> CalculateShortestDistance(Node start)
    {
      ISet<Node> Unvisited = new HashSet<Node>();
      Node? end = null;
      Node? curNode;

      for (int x = 0; x < sizeX; x++)
      {
        for (int y = 0; y < sizeY; y++)
        {
          curNode = Grid[y][x];
          curNode.PreviousNode = null;
          if (curNode == start)
          {
            start = curNode;
            start.DistanceFromStart = 0;
          }
          else
          {
            if (curNode.Elevation == 'E')
            {
              end = curNode;
            }
            curNode.DistanceFromStart = int.MaxValue;
            Unvisited.Add(curNode);
          }
        }
      }

      if (start == null || end == null)
      {
        throw new InvalidOperationException("No start or end in input!");
      }

      curNode = start;

      while (Unvisited.Contains(end))
      {
        List<Node> neighbors = GetSurroundingNodes(curNode);
        foreach (Node neighbor in neighbors)
        {
          if (neighbor.IsReachableFromElevation(curNode.NumericElevation) && Unvisited.Contains(neighbor))
          {
            int newDistance = curNode.DistanceFromStart + 1;
            if (newDistance < neighbor.DistanceFromStart)
            {
              neighbor.DistanceFromStart = newDistance;
              neighbor.PreviousNode = curNode;
            }
          }
        }
        Unvisited.Remove(curNode);
        curNode = Unvisited.OrderBy(n => n.DistanceFromStart).First();
        if (curNode.DistanceFromStart == int.MaxValue)
        {
          break;
        }
      }

      List<Node> path = new();
      curNode = end;
      while (curNode != null)
      {
        path.Add(curNode);
        curNode = curNode.PreviousNode;
      }
      path.Reverse();

      return path;
    }

    private List<Node> GetSurroundingNodes(Node curNode)
    {
      List<Node> surrounding = new();

      AddIfNotNull(surrounding, GetNodeAtOffset(curNode, -1, 0));
      AddIfNotNull(surrounding, GetNodeAtOffset(curNode, 1, 0));
      AddIfNotNull(surrounding, GetNodeAtOffset(curNode, 0, -1));
      AddIfNotNull(surrounding, GetNodeAtOffset(curNode, 0, 1));

      return surrounding;
    }

    private Node? GetNodeAtOffset(Node curNode, int dx, int dy)
    {
      int actualX = curNode.X + dx;
      int actualY = curNode.Y + dy;
      if (actualX >= 0 && actualX < sizeX && actualY >= 0 && actualY < sizeY)
      {
        return Grid[actualY][actualX];
      }
      return null;
    }

    private Node[][] ParseGrid(string input)
    {
      List<Node[]> lines = new();
      int y = 0;
      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        List<Node> nodeLine = new();
        for (int x = 0; x < line.Length; x++)
        {
          nodeLine.Add(new Node(line[x], x, y));
        }
        lines.Add(nodeLine.ToArray());
        y++;
      }
      return lines.ToArray();
    }

    private static void AddIfNotNull(List<Node> list, Node? node)
    {
      if (node != null)
      {
        list.Add(node);
      }
    }
  }
}