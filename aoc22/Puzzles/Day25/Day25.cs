using aoc_common;
using System;

namespace aoc22.Puzzles.Day25
{
  public class Day25 : IPuzzle
  {
    public string PuzzleName => "Day 25: Full of Hot Air";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      long sum = 0;
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        long decNum = SnafuConverter.FromSnafu(inputLine);
#if DEBUG
        Console.WriteLine($"{inputLine, 30} : {decNum, 30} : {SnafuConverter.ToSnafu(decNum), 30}");
#endif
        sum += decNum;
      }

      Console.WriteLine($"The sum is {sum}, which is '{SnafuConverter.ToSnafu(sum)}' in SNAFU.");
    }
  }
}
