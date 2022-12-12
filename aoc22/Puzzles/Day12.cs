using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static aoc22.Puzzles.Day7;

namespace aoc22.Puzzles
{
  internal class Day12 : IPuzzleSolver
  {
    public string PuzzleName => "Day 12: Hill Climbing Algorithm";

    public string SolvePart1(string input)
    {
      Dijkstra dijkstra = new(input);
      List<Node> path = dijkstra.CalculateShortestDistance(dijkstra.GetAllNodesWithElevation('S').First());
      
      return $"The smallest distance from the start is: {path.Last().DistanceFromStart}.";
    }

    public string SolvePart2(string input)
    {
      Dijkstra dijkstra = new(input);
      int bestDistance = int.MaxValue;
      foreach (Node start in dijkstra.GetAllNodesWithElevation('a').Concat(dijkstra.GetAllNodesWithElevation('S')))
      {
        List<Node> path = dijkstra.CalculateShortestDistance(start);
        if (path.Last().DistanceFromStart < bestDistance)
        {
          bestDistance = path.Last().DistanceFromStart;
        }
      }

      return $"The smallest distance from the start is: {bestDistance}.";
    }

    private sealed class Dijkstra
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

    private sealed class Node
    {
      public char Elevation { get; }
      public int NumericElevation { get; }
      public int X { get; }
      public int Y { get; }
      public int DistanceFromStart { get; set; }
      public Node? PreviousNode { get; set; }

      public Node(char elevation, int x, int y)
      {
        X = x;
        Y = y;
        Elevation = elevation;
        if (elevation == 'S')
        {
          NumericElevation = 'a' - 'a';
        }
        else if (elevation == 'E')
        {
          NumericElevation = 'z' - 'a';
        }
        else
        {
          NumericElevation = elevation - 'a';
        }
      }

      public bool IsReachableFromElevation(int OtherElevation)
      {
        return NumericElevation - OtherElevation < 2;
      }

      public override string ToString()
      {
        return $"Node: {Elevation}; Distance: {DistanceFromStart}; ({X},{Y})";
      }
    }
  }
}
