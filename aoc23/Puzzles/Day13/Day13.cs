using aoc_common;

namespace aoc23.Puzzles.Day13
{
  public class Day13 : IPuzzle
  {
    public string PuzzleName => "Day 13: Point of Incidence";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<Pattern> patterns = [];
      foreach (var inputPattern in input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        patterns.Add(new(inputPattern));
      }

      Console.WriteLine("Part 1:");
      long sum = 0;
      foreach (var pattern in patterns)
      {
        int horIndex = pattern.FindHorizontalMirrorIndex();
        if (horIndex >= 0)
        {
          Console.WriteLine($"Found horizontal index: {horIndex}");
          sum += horIndex;
        }

        int vertIndex = pattern.FindVerticalMirrorIndex();
        if (vertIndex >= 0)
        {
          Console.WriteLine($"Found vertical index: {vertIndex}");
          sum += 100 * vertIndex;
        }
      }
      Console.WriteLine($"Sum of mirror indices is: {sum}");
    }
  }
}
