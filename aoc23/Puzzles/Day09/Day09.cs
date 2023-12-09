using aoc_common;

namespace aoc23.Puzzles.Day09
{
  public class Day09 : IPuzzle
  {
    public string PuzzleName => "Day 9: Mirage Maintenance";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<History> histories = [];
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        histories.Add(new(line));
      }

      Part1(histories);
      Part2(histories);
    }

    private static void Part1(List<History> histories)
    {
      Console.WriteLine("Part 1:");
      int sum = 0;
      foreach (var hist in histories)
      {
        int prediction = hist.PredictNextValue();
        Console.WriteLine(prediction);
        sum += prediction;
      }
      Console.WriteLine($"Sum of predicted next values: {sum}");
    }

    private static void Part2(List<History> histories)
    {
      Console.WriteLine("Part 2:");
      int sum = 0;
      foreach (var hist in histories)
      {
        int prediction = hist.PredictPreviousValue();
        Console.WriteLine(prediction);
        sum += prediction;
      }
      Console.WriteLine($"Sum of predicted previous values: {sum}");
    }
  }
}
