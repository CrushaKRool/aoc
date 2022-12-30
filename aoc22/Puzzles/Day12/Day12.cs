using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day12
{
  internal class Day12 : IPuzzle
  {
    public string PuzzleName => "Day 12: Hill Climbing Algorithm";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Dijkstra dijkstra = new(input);
      List<Node> path = dijkstra.CalculateShortestDistance(dijkstra.GetAllNodesWithElevation('S').First());

      Console.WriteLine($"The smallest distance from the start S is: {path.Last().DistanceFromStart}.");

      // Part 2

      dijkstra = new(input);
      int bestDistance = int.MaxValue;
      foreach (Node start in dijkstra.GetAllNodesWithElevation('a').Concat(dijkstra.GetAllNodesWithElevation('S')))
      {
        path = dijkstra.CalculateShortestDistance(start);
        if (path.Last().DistanceFromStart < bestDistance)
        {
          bestDistance = path.Last().DistanceFromStart;
        }
      }

      Console.WriteLine($"The smallest distance from any lowest elevation is: {bestDistance}.");
    }
  }
}