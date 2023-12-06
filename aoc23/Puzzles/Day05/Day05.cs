using aoc_common;
using System.Diagnostics;
using System.Net;

namespace aoc23.Puzzles.Day05
{
  public class Day05 : IPuzzle
  {
    public string PuzzleName => "Day 5: If You Give A Seed A Fertilizer";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      long[] seeds = [];
      List<Mapping> mappings = [];
      Mapping? curMapping = null;
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.TrimEntries))
      {
        if (inputLine.StartsWith("seeds: "))
        {
          seeds = inputLine["seeds: ".Length..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }
        else if (inputLine.EndsWith(" map:"))
        {
          curMapping = new Mapping(inputLine[..^" map:".Length]);
          mappings.Add(curMapping);
        }
        else if (string.IsNullOrWhiteSpace(inputLine))
        {
          curMapping = null;
        }
        else
        {
          Trace.Assert(curMapping != null);
          curMapping.Ranges.Add(new(inputLine));
        }
      }

      Part1(mappings, seeds);
    }

    private static void Part1(List<Mapping> mappings, long[] seeds)
    {
      Console.WriteLine("Part 1:");
      long lowestResult = int.MaxValue;
      foreach (long seed in seeds)
      {
        Console.Write($"Seed {seed}");
        long curValue = seed;
        foreach (Mapping mapping in mappings)
        {
          curValue = mapping.MapValue(curValue);
          Console.Write($" -> {mapping.Name} {curValue}");
        }
        Console.WriteLine();
        lowestResult = Math.Min(lowestResult, curValue);
      }
      Console.WriteLine($"The lowest result is {lowestResult}.");
    }
  }
}
