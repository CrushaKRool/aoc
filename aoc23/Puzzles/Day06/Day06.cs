using aoc_common;
using aoc23.Puzzles.Day05;
using System.Diagnostics;

namespace aoc23.Puzzles.Day06
{
  public class Day06 : IPuzzle
  {
    public string PuzzleName => "Day 6: Wait For It";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);

      Console.WriteLine("--------------------------------------------");

      Part2(input);
    }

    static void Part1(string input)
    {
      Console.WriteLine("Part 1:");

      List<Race> races = [];
      string[] inputLines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      int[] time = inputLines[0]["Time:".Length..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
      int[] distance = inputLines[1]["Distance:".Length..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
      Trace.Assert(time.Length == distance.Length);

      for (int i = 0; i < time.Length; i++)
      {
        races.Add(new Race(time[i], distance[i]));
      }

      long product = 1;
      foreach (var race in races)
      {
        long permutation = race.GetNumRecordBeatingPermutations();
        Console.WriteLine($"Permutations: {permutation}");
        product *= permutation;
      }
      Console.WriteLine($"Product of permutations: {product}");
    }

    static void Part2(string input)
    {
      Console.WriteLine("Part 2:");

      string[] inputLines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      long time = long.Parse(inputLines[0]["Time:".Length..].Replace(" ", ""));
      long distance = long.Parse(inputLines[1]["Distance:".Length..].Replace(" ", ""));

      Race race = new(time, distance);
      long permutations = race.GetNumRecordBeatingPermutations();
      Console.WriteLine($"Number of permuations: {permutations}");
    }
  }
}
