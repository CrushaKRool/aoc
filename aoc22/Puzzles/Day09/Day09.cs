using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day09
{
  internal class Day09 : IPuzzleSolver
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

  internal class RopeBridgeState
  {
    private List<Vector2> RopeSegments { get; } = new();

    // Convenience accessors.
    private Vector2 Head { get => RopeSegments[0]; set => RopeSegments[0] = value; }

    private Vector2 Tail { get => RopeSegments[RopeSegments.Count - 1]; set => RopeSegments[RopeSegments.Count - 1] = value; }

    /// <summary>
    /// Returns a set that contains all coordinates that the last rope segment visited.
    /// </summary>
    public ISet<Vector2> VisitedTailPositions { get; } = new HashSet<Vector2>();

    public RopeBridgeState(int numRopeSegments)
    {
      if (numRopeSegments < 2)
      {
        throw new ArgumentException("Rope must consist of at least two segments.");
      }

      for (int i = 0; i < numRopeSegments; i++)
      {
        RopeSegments.Add(new Vector2());
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
      Head += new Vector2(dx, dy);
      UpdateTail();
      RecordVisitedTailLocation();
    }

    private void UpdateTail()
    {
      for (int i = 0; i < RopeSegments.Count - 1; i++)
      {
        // Get values from list, so we can pass them as ref parameters.
        Vector2 tailPoint = RopeSegments[i + 1];

        UpdateTailSegment(RopeSegments[i], ref tailPoint);

        // Write updated values back to list.
        RopeSegments[i + 1] = tailPoint;
      }
    }

    private void UpdateTailSegment(Vector2 headPoint, ref Vector2 tailPoint)
    {
      if (Math.Abs(headPoint.X - tailPoint.X) < 2 && Math.Abs(headPoint.Y - tailPoint.Y) < 2)
      {
        // Tail is still within range of head.
        return;
      }

      // Get us the general direction in which we want to move.
      Vector2 moveDir = headPoint - tailPoint;
      // Make sure that we never move more than one in any direction.
      // Also round to integers, just in case...
      moveDir.X = (float)Math.Clamp(Math.Round(moveDir.X, MidpointRounding.AwayFromZero), -1, 1);
      moveDir.Y = (float)Math.Clamp(Math.Round(moveDir.Y, MidpointRounding.AwayFromZero), -1, 1);
      tailPoint += moveDir;
    }

    private void RecordVisitedTailLocation()
    {
      VisitedTailPositions.Add(Tail);
    }
  }
}