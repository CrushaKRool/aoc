using aoc_common;

namespace aoc23.Puzzles.Day02
{
  public class Day02 : IPuzzle
  {
    public string PuzzleName => "Day 02: Cube Conundrum";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Console.WriteLine("Part 1:");
      Part1(input);

      Console.WriteLine("-----------------------------------------------------");

      Console.WriteLine("Part 2:");
      Part2(input);
    }

    private static void Part1(string input)
    {
      long sum = 0;
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        GameData game = new(line);
        if (IsGamePossible(game, 12, 13, 14))
        {
          Console.WriteLine($"Game {game.GameNumber} is possible.");
          sum += game.GameNumber;
        }
      }
      Console.WriteLine("Sum of Game IDs: " + sum);
    }

    private static bool IsGamePossible(GameData game, int availableRed, int availableGreen, int availableBlue)
    {
      return game.MaxRed <= availableRed
        && game.MaxGreen <= availableGreen
        && game.MaxBlue <= availableBlue;
    }

    private static void Part2(string input)
    {
      long sum = 0;
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        GameData game = new(line);
        Console.WriteLine($"Power of minimum set of cubes for game {game.GameNumber} is {game.PowerOfMinimumSet}.");
        sum += game.PowerOfMinimumSet;
      }
      Console.WriteLine("Sum of Powers: " + sum);
    }
  }
}
