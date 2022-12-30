using aoc_common;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day09
{
  internal class Day09 : IPuzzle
  {
    private readonly Regex CommandParser = new("(\\w) (\\d+)");

    public string PuzzleName => "Day 9: Rope Bridge";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      RopeBridgeState state = new(2);

      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        ProcessMoveCommand(line, state);
      }

      Console.WriteLine($"The rope tail (2) visited {state.VisitedTailPositions.Count} different positions.");

      // Part 2

      state = new(10);

      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        ProcessMoveCommand(line, state);
      }

      Console.WriteLine($"The rope tail (10) visited {state.VisitedTailPositions.Count} different positions.");
    }

    private void ProcessMoveCommand(string line, RopeBridgeState state)
    {
      Match m = CommandParser.Match(line);
      if (m.Success)
      {
        string moveDir = m.Groups[1].Value.ToUpper();
        int moveCount = int.Parse(m.Groups[2].Value);
        for (int i = 0; i < moveCount; i++)
        {
          switch (moveDir)
          {
            case "U":
              state.MoveHeadUp();
              break;

            case "D":
              state.MoveHeadDown();
              break;

            case "L":
              state.MoveHeadLeft();
              break;

            case "R":
              state.MoveHeadRight();
              break;

            default:
              throw new ArgumentException($"Invalid move direction: {moveDir}");
          }
        }
      }
    }
  }
}