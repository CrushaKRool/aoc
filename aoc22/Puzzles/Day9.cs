using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace aoc22.Puzzles
{
  class Day9 : IPuzzleSolver
  {
    private readonly Regex CommandParser = new("(\\w) (\\d+)");

    public string PuzzleName => "Day 9: Rope Bridge";

    public string SolvePart1(string input)
    {
      RopeBridgeState state = new(2);

      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        ProcessMoveCommand(line, state);
      }

      return $"The rope tail visited {state.VisitedTailPositions.Count} different positions.";
    }

    public string SolvePart2(string input)
    {
      RopeBridgeState state = new(10);

      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        ProcessMoveCommand(line, state);
      }

      return $"The rope tail visited {state.VisitedTailPositions.Count} different positions.";
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

  class RopeBridgeState
  {
    private List<Point> RopeSegments { get; } = new();

    // Convenience accessors.
    private Point Head { get => RopeSegments[0]; set => RopeSegments[0] = value; }
    private Point Tail { get => RopeSegments[RopeSegments.Count - 1]; set => RopeSegments[RopeSegments.Count - 1] = value; }

    /// <summary>
    /// Returns a set that contains all coordinates that the last rope segment visited.
    /// </summary>
    public ISet<Point> VisitedTailPositions { get; } = new HashSet<Point>();

    public RopeBridgeState(int numRopeSegments)
    {
      if (numRopeSegments < 2)
      {
        throw new ArgumentException("Rope must consist of at least two segments.");
      }

      for (int i = 0; i < numRopeSegments; i++)
      {
        RopeSegments.Add(new Point());
      }

      // Record the initial starting position of the tail as visited.
      RecordVisitedTailLocation();
    }

    public void MoveHeadUp()
    {
      MoveHead(0, 1);
    }

    public void MoveHeadDown()
    {
      MoveHead(0, -1);
    }

    public void MoveHeadLeft()
    {
      MoveHead(-1, 0);
    }

    public void MoveHeadRight()
    {
      MoveHead(1, 0);
    }

    private void MoveHead(int dx, int dy)
    {
      Head += new Vector(dx, dy);
      UpdateTail();
      RecordVisitedTailLocation();
    }

    private void UpdateTail()
    {
      for (int i = 0; i < RopeSegments.Count - 1; i++)
      {
        // Get values from list, so we can pass them as ref parameters.
        Point tailPoint = RopeSegments[i + 1];

        UpdateTailSegment(RopeSegments[i], ref tailPoint);

        // Write updated values back to list.
        RopeSegments[i + 1] = tailPoint;
      }
    }

    private void UpdateTailSegment(Point headPoint, ref Point tailPoint)
    {
      if (Math.Abs(headPoint.X - tailPoint.X) < 2 && Math.Abs(headPoint.Y - tailPoint.Y) < 2)
      {
        // Tail is still within range of head.
        return;
      }

      // Get us the general direction in which we want to move.
      Vector moveDir = headPoint - tailPoint;
      // Make sure that we never move more than one in any direction.
      // Also round to integers, just in case...
      moveDir.X = Math.Clamp(Math.Round(moveDir.X, MidpointRounding.AwayFromZero), -1, 1);
      moveDir.Y = Math.Clamp(Math.Round(moveDir.Y, MidpointRounding.AwayFromZero), -1, 1);
      tailPoint += moveDir;
    }

    private void RecordVisitedTailLocation()
    {
      VisitedTailPositions.Add(Tail);
    }
  }
}
