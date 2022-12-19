#undef TRACE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day19
{
  internal class Blueprint
  {
    private static readonly Regex BlueprintIdParser = new("Blueprint (\\d+): ");
    private static readonly Regex RobotRecipeParser = new("\\s*(ore|clay|obsidian|geode) robot costs (.+)\\.");

    public int ID { get; init; }
    public IList<RobotRecipe> RobotRecipes { get; } = new List<RobotRecipe>();

    public Blueprint(string inputLine)
    {
      string[] parts = inputLine.Split("Each");
      ID = int.Parse(BlueprintIdParser.Match(parts[0]).Groups[1].Value);
      for (int i = 1; i < parts.Length; i++)
      {
        Match m = RobotRecipeParser.Match(parts[i]);

        Mineral miningType = m.Groups[1].Value switch
        {
          "ore" => Mineral.Ore,
          "clay" => Mineral.Clay,
          "obsidian" => Mineral.Obsidian,
          "geode" => Mineral.Geode,
          _ => throw new ArgumentException("Unknown mineral type.")
        };

        RobotRecipe recipe = new RobotRecipe() { MiningType = miningType };
        string[] resourceCosts = m.Groups[2].Value.Split(" and ");
        foreach (string res in resourceCosts)
        {
          string[] split = res.Split(" ");
          switch (split[1])
          {
            case "ore":
              recipe.Costs[Mineral.Ore] = int.Parse(split[0]);
              break;
            case "clay":
              recipe.Costs[Mineral.Clay] = int.Parse(split[0]);
              break;
            case "obsidian":
              recipe.Costs[Mineral.Obsidian] = int.Parse(split[0]);
              break;
            default:
              throw new ArgumentException("Unknown mineral type in recipe.");
          }
        }
        RobotRecipes.Add(recipe);
      }
    }

    public long CalculateQualityLevel(int minutesRemaining)
    {
      return CalculateGeodeCount(minutesRemaining) * ID;
    }

    public long CalculateGeodeCount(int minutesRemaining)
    {
      TimeState root = new TimeState(null, null, minutesRemaining, this);
      root.PopulateNextStates();

      List<TimeState> leaves = root.GetLeafStates().ToList();
      TimeState? bestState = leaves.MaxBy(ts => ts.Resources[Mineral.Geode]);
      if (bestState != null)
      {
#if TRACE
        List<TimeState> bestLeaves = leaves.Where(l => l.Resources[Mineral.Geode] == bestState.Resources[Mineral.Geode]).ToList();
        foreach (TimeState ts in bestLeaves)
        {
          PrintRoutToState(ts);
        }
#elif DEBUG
        PrintRoutToState(bestState);
#endif
        return bestState.Resources[Mineral.Geode];
      }
      return 0;
    }

    private void PrintRoutToState(TimeState end)
    {
      List<TimeState> bestPath = new();
      TimeState? cur = end;
      while (cur != null)
      {
        bestPath.Add(cur);
        cur = cur.PreviousState;
      }
      bestPath.Reverse();
      int i = 0;
      foreach (TimeState ts in bestPath)
      {
        Console.WriteLine($"Minute {i++}; {ts}");
      }
      Console.WriteLine($"{end.Resources[Mineral.Geode]} geodes cracked.");
    }
  }
}
