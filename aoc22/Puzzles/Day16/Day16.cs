using aoc_common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc22.Puzzles.Day16
{
  public class Day16 : IPuzzle
  {
    public string PuzzleName => "Day 16: Proboscidea Volcanium";

    public string InputFileName => @"Puzzles\Day16\Day16Input.txt";

    public void Run(string input)
    {
      Graph graph = new();
      foreach (string inputLine in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)))
      {
        graph.AddNode(inputLine);
      }

      foreach (Node startNode in graph.Nodes.Values.Where(n => n.FlowRate > 0 || n.Name == "AA"))
      {
        foreach (Node endNode in graph.Nodes.Values.Where(n => n.FlowRate > 0))
        {
          if (startNode == endNode)
          {
            continue;
          }
          ShortestPath pathBetween = graph.CalculateShortestDistance(startNode, endNode);
          startNode.ShortestPaths.Add(pathBetween);
        }
      }

      Part1(graph);
      foreach (Node node in graph.Nodes.Values)
      {
        node.ValveOpened = false;
      }
      Console.WriteLine("-----------------------------------------------");
      Part2(graph);
    }

    private void Part1(Graph graph)
    {
      // Per puzzle description, AA is the start with the elefants in the room.
      Node current = graph.Nodes["AA"];
      const int startTime = 30;
      int remainingTime = startTime;
      int releasedPressure = 0;
      ShortestPath? activePath = null;
      while (remainingTime > 0)
      {
        Console.WriteLine($"Minute {startTime - remainingTime + 1}");
        int totalFlow = GetTotalFlowRate(graph);
        releasedPressure += totalFlow;
        Console.WriteLine($"Remaining time: {remainingTime}; Flow rate: {totalFlow}; Released pressure: {releasedPressure}");

        if (current.FlowRate > 0 && !current.ValveOpened) // Open valve
        {
          Console.WriteLine($"Opening valve: {current.Name}; flow: {current.FlowRate}");
          current.ValveOpened = true;
        }
        else // Move
        {
          if (activePath == null)
          {
            activePath = current.ShortestPaths.OrderByDescending(sp => sp.GetEstimatedValue(remainingTime, current)).First();
            Console.WriteLine($"Choosing new active path with value {activePath.GetEstimatedValue(remainingTime, current)}:" +
              $" {string.Join("->", activePath.PathNodes.Select(n => n.Name))}");
          }
          int currIndex = activePath.PathNodes.IndexOf(current);
          if (currIndex == activePath.PathNodes.Count - 1)
          {
            activePath = current.ShortestPaths.OrderByDescending(sp => sp.GetEstimatedValue(remainingTime, current)).First();
            Console.WriteLine($"Reached path end. Choosing new active path with value {activePath.GetEstimatedValue(remainingTime, current)}:" +
              $" {string.Join("->", activePath.PathNodes.Select(n => n.Name))}");
            currIndex = activePath.PathNodes.IndexOf(current);
          }
          current = activePath.PathNodes[currIndex + 1];
          Console.WriteLine($"Moving to node: {current.Name}");
        }
        remainingTime--;
      }

      Console.WriteLine($"The most pressure that can be released alone is {releasedPressure}.");
    }

    private void Part2(Graph graph)
    {
      // Per puzzle description, AA is the start with the elefants in the room.
      Node currentA = graph.Nodes["AA"];
      Node currentB = graph.Nodes["AA"];
      // Spend 4 initial minutes teaching the elefant.
      const int startTime = 26;
      int remainingTime = startTime;
      int releasedPressure = 0;
      ShortestPath? activePathA = null;
      ShortestPath? activePathB = null;

      while (remainingTime > 0)
      {
        Console.WriteLine($"Minute {startTime - remainingTime + 1}");
        int totalFlow = GetTotalFlowRate(graph);
        releasedPressure += totalFlow;
        Console.WriteLine($"Remaining time: {remainingTime}; Flow rate: {totalFlow}; Released pressure: {releasedPressure}");

        bool aOpened = false;
        bool bOpened = false;

        if (currentA.FlowRate > 0 && !currentA.ValveOpened) // Open valve
        {
          Console.WriteLine($"A: Opening valve: {currentA.Name}; flow: {currentA.FlowRate}");
          currentA.ValveOpened = true;
          aOpened = true;
        }
        if (currentB.FlowRate > 0 && !currentB.ValveOpened) // Open valve
        {
          Console.WriteLine($"A: Opening valve: {currentB.Name}; flow: {currentB.FlowRate}");
          currentB.ValveOpened = true;
          bOpened = true;
        }

        // Choose initial path if both are unset.
        if (activePathA == null || activePathB == null)
        {
          GetOptimalPathPair(remainingTime, currentA, currentB, ref activePathA, ref activePathB);
          Console.WriteLine($"A: Choosing new active path with value {activePathA.GetEstimatedValue(remainingTime, currentA)}:" +
            $" {string.Join("->", activePathA.PathNodes.Select(n => n.Name))}");
          Console.WriteLine($"B: Choosing new active path with value {activePathB.GetEstimatedValue(remainingTime, currentB)}:" +
            $" {string.Join("->", activePathB.PathNodes.Select(n => n.Name))}");
        }
        if (!aOpened) // Move
        {
          int currIndex = activePathA.PathNodes.IndexOf(currentA);
          if (currIndex == activePathA.PathNodes.Count - 1)
          {
            activePathA = null;
            GetOptimalPathPair(remainingTime, currentA, currentB, ref activePathA, ref activePathB);
            Console.WriteLine($"A: Choosing new active path with value {activePathA.GetEstimatedValue(remainingTime, currentA)}:" +
              $" {string.Join("->", activePathA.PathNodes.Select(n => n.Name))}");
            currIndex = activePathA.PathNodes.IndexOf(currentA);
          }
          currentA = activePathA.PathNodes[currIndex + 1];
          Console.WriteLine($"A: Moving to node: {currentA.Name}");
        }
        if (!bOpened)
        {
          int currIndex = activePathB.PathNodes.IndexOf(currentB);
          if (currIndex == activePathB.PathNodes.Count - 1)
          {
            activePathB = null;
            GetOptimalPathPair(remainingTime, currentA, currentB, ref activePathA, ref activePathB);
            Console.WriteLine($"B: Choosing new active path with value {activePathB.GetEstimatedValue(remainingTime, currentB)}:" +
              $" {string.Join("->", activePathB.PathNodes.Select(n => n.Name))}");
            currIndex = activePathB.PathNodes.IndexOf(currentA);
          }
          currentB = activePathB.PathNodes[currIndex + 1];
          Console.WriteLine($"B: Moving to node: {currentB.Name}");
        }
        remainingTime--;
      }

      Console.WriteLine($"The most pressure that can be released with the elefant is {releasedPressure}.");
    }

    private static int GetTotalFlowRate(Graph graph)
    {
      return graph.Nodes.Values.Where(n => n.ValveOpened).Sum(n => n.FlowRate);
    }

    private static void GetOptimalPathPair(int remainingTime, Node currentA, Node currentB, ref ShortestPath? pathA, ref ShortestPath? pathB)
    {
      List<PathPair> pairs = new();
      if (pathA == null && pathB == null)
      {
        foreach (ShortestPath candidateA in currentA.ShortestPaths)
        {
          foreach (ShortestPath candidateB in currentB.ShortestPaths)
          {
            if (candidateA == candidateB || PathsOverlap(candidateA, candidateB))
            {
              continue;
            }
            PathPair pair = new(candidateA, candidateB);
            pairs.Add(pair);
          }
        }
        PathPair bestPair = pairs.MaxBy(p => p.GetCombinedValue(remainingTime, currentA, currentB));
        pathA = bestPair.PathA;
        pathB = bestPair.PathB;
        return;
      }
      if (pathA == null)
      {
        foreach (ShortestPath candidateA in currentA.ShortestPaths)
        {
          if (candidateA == pathB || PathsOverlap(candidateA, pathB))
          {
            continue;
          }
          PathPair pair = new(candidateA, pathB);
          pairs.Add(pair);
        }
        if (pairs.Count == 0) // Fallback: Relax overlapping constraint.
        {
          foreach (ShortestPath candidateA in currentA.ShortestPaths)
          {
            if (candidateA == pathB)
            {
              continue;
            }
            PathPair pair = new(candidateA, pathB);
            pairs.Add(pair);
          }
        }
        PathPair bestPair = pairs.MaxBy(p => p.GetCombinedValue(remainingTime, currentA, currentB));
        pathA = bestPair.PathA;
        pathB = bestPair.PathB;
        return;
      }
      if (pathB == null)
      {
        foreach (ShortestPath candidateB in currentB.ShortestPaths)
        {
          if (pathA == candidateB || PathsOverlap(pathA, candidateB))
          {
            continue;
          }
          PathPair pair = new(pathA, candidateB);
          pairs.Add(pair);
        }
        if (pairs.Count == 0) // Fallback: Relax overlapping constraint.
        {
          foreach (ShortestPath candidateB in currentB.ShortestPaths)
          {
            if (pathA == candidateB)
            {
              continue;
            }
            PathPair pair = new(pathA, candidateB);
            pairs.Add(pair);
          }
        }
        PathPair bestPair = pairs.MaxBy(p => p.GetCombinedValue(remainingTime, currentA, currentB));
        pathA = bestPair.PathA;
        pathB = bestPair.PathB;
      }
    }

    private static bool PathsOverlap(ShortestPath pathA, ShortestPath pathB)
    {
      return pathA.PathNodes.Where(n => n.FlowRate > 0 && !n.ValveOpened).Intersect(pathB.PathNodes.Where(n => n.FlowRate > 0 && !n.ValveOpened)).Any();
    }
  }
}