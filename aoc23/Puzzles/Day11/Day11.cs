using aoc_common;

namespace aoc23.Puzzles.Day11
{
  public class Day11 : IPuzzle
  {
    public string PuzzleName => "Day 11: Cosmic Expansion";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      Universe universe = new(input);

      universe.Expand(2);
      universe.Visualize();

      long distanceSum = 0;

      var pairs = universe.GetGalaxyPairs();
      foreach (var pair in pairs)
      {
        long distance = pair.Item1.DistanceToOther(pair.Item2);
        distanceSum += distance;
      }

      Console.WriteLine($"Sum of shortest paths: {distanceSum}");
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      Universe universe = new(input);

      universe.Expand(1000000);

      long distanceSum = 0;

      var pairs = universe.GetGalaxyPairs();
      foreach (var pair in pairs)
      {
        long distance = pair.Item1.DistanceToOther(pair.Item2);
        distanceSum += distance;
      }

      Console.WriteLine($"Sum of shortest paths: {distanceSum}");
    }
  }
}
