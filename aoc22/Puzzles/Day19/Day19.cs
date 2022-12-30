using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day19
{
  public class Day19 : IPuzzle
  {
    public string PuzzleName => "Day 19: Not Enough Minerals";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<Blueprint> blueprints = new();
      foreach (string line in input.Split(Environment.NewLine))
      {
        blueprints.Add(new(line));
      }

      long total = 0;
      foreach (Blueprint bp in blueprints)
      {
        Console.WriteLine($"Calculating quality of blueprint {bp.ID}.");
        total += bp.CalculateQualityLevel(24);
        Console.WriteLine($"Total Quality Level is now {total}.");
      }

      total = 1;
      foreach (Blueprint bp in blueprints.Take(3))
      {
        Console.WriteLine($"Calculating geode count of blueprint {bp.ID}.");
        total *= bp.CalculateGeodeCount(32);
        Console.WriteLine($"Geode product is now {total}.");
      }
    }
  }
}
