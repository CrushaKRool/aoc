using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day24
{
  public class Day24 : IPuzzle
  {
    public string PuzzleName => "Day 24: Blizzard Basin";

    public string InputFileName => @"Puzzles\Day24\Day24Input.txt";

    public void Run(string input)
    {
      Board board = new(input);
      PositionState? goal = board.FindShortestPath(1, 0, board.Width - 2, board.Height - 1, 1);


      if (goal == null)
      {
        throw new ArgumentException("No path found on first trip!");
      }
#if DEBUG
      PrintPathToGoal(board, goal);
#endif
      int firstTripDuration = goal.Tick;
      Console.WriteLine($"Found exit after {firstTripDuration} minutes.");

      // Part 2.

      int secondTripDuration;
      int thirdTripDuration;

      goal = board.FindShortestPath(board.Width - 2, board.Height - 1, 1, 0, goal.Tick);
      if (goal == null)
      {
        throw new ArgumentException("No path found on second trip!");
      }
#if DEBUG
      PrintPathToGoal(board, goal);
#endif
      secondTripDuration = goal.Tick - firstTripDuration;

      Console.WriteLine($"Went back to start in another {secondTripDuration} minutes.");

      goal = board.FindShortestPath(1, 0, board.Width - 2, board.Height - 1, goal.Tick);
      if (goal == null)
      {
        throw new ArgumentException("No path found on third trip!");
      }
#if DEBUG
      PrintPathToGoal(board, goal);
#endif
      thirdTripDuration = goal.Tick - firstTripDuration - secondTripDuration;

      Console.WriteLine($"Found exit again after another {thirdTripDuration} minutes.");
      Console.WriteLine($"Total duration: {firstTripDuration + secondTripDuration + thirdTripDuration}.");
    }

    private static void PrintPathToGoal(Board board, PositionState? goal)
    {
      List<PositionState> path = new();
      PositionState? current = goal;
      while (current != null)
      {
        path.Add(current);
        current = current.Parent;
      }
      path.Reverse();

      foreach (PositionState ps in path)
      {
        Console.WriteLine($"=== Minute {ps.Tick} ===");
        board.PrintPositionState(ps);
        Console.ReadKey(true);
      }
    }
  }
}
