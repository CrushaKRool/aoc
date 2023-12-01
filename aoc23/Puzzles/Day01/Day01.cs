using aoc_common;
using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day01
{
  public partial class Day01 : IPuzzle
  {
    public string PuzzleName => "Day 01: Trebuchet?!";

    public string InputFileName => @"Input.txt";

    private static readonly Regex digitRegex = DigitRegex();

    public void Run(string input)
    {
      Console.WriteLine("Part 1:");
      Part1(input);

      Console.WriteLine("-----------------------------------------------------");

      Console.WriteLine("Part 2:");
      Part2(input);
    }

    private void Part1(string input)
    {
      long sum = 0;
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        char firstDigit = line.First(char.IsDigit);
        char lastDigit = line.Last(char.IsDigit);
        int num = int.Parse(firstDigit + "" + lastDigit);
        sum += num;
        Console.WriteLine(line + " => " + num);
      }
      Console.WriteLine("Sum: " + sum);
    }

    private void Part2(string input)
    {
      long sum = 0;
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        int firstDigit = MatchToNumber(digitRegex.Matches(line).First());
        int lastDigit = MatchToNumber(digitRegex.Matches(line).Last());
        int num = int.Parse(firstDigit + "" + lastDigit);
        sum += num;
        Console.WriteLine(line + " => " + num);
      }
      Console.WriteLine("Sum: " + sum);
    }

    private int MatchToNumber(Match match)
    {
      string captureValue = match.Groups[1].Value;
      return captureValue switch
      {
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        _ => int.Parse(captureValue)
      };
    }

    // We need a positive look-ahead here in order to correctly find both numbers in case of overlapping words, like "oneight".
    [GeneratedRegex(@"(?=(one|two|three|four|five|six|seven|eight|nine|\d))")]
    private static partial Regex DigitRegex();
  }
}
