using aoc_common;
using System.Linq;

namespace aoc23.Puzzles.Day12
{
  public class Day12 : IPuzzle
  {
    public string PuzzleName => "Day 12: Hot Springs";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Console.WriteLine("-------------------------------------------");
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      List<ConditionRecord> records = [];
      foreach (var inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        records.Add(new(inputLine));
      }

      int sumOfArrangements = records.Sum(GetArrangementCount);
      Console.WriteLine($"Sum of arrangements: {sumOfArrangements}");
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      List<ConditionRecord> records = [];
      foreach (var inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        var rec = new ConditionRecord(inputLine);
        rec.Unfold();
        records.Add(rec);
      }

      int sumOfArrangements = records.AsParallel().Sum(GetArrangementCount);
      Console.WriteLine($"Sum of arrangements: {sumOfArrangements}");
    }

    private static int GetArrangementCount(ConditionRecord rec)
    {
      int arrangements = rec.GetPermutationsOfCondition().Count(perm => rec.ConditionMatchesDamagedGroups(perm));
      Console.WriteLine($"{rec.Condition} - {string.Join(",", rec.DamagedGroups)} has {arrangements} arrangements");
      return arrangements;
    }
  }
}
