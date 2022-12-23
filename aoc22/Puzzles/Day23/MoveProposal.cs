using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace aoc22.Puzzles.Day23
{
  internal class MoveProposal
  {
    public CardinalDirection MoveDirection { get; init; }
    public CardinalDirection[] CheckDirections { get; init; }

    public MoveProposal(CardinalDirection moveDir, params CardinalDirection[] checkDirs)
    {
      MoveDirection = moveDir;
      CheckDirections = checkDirs;
    }

    public bool CanMove(Elf elfToMove, IEnumerable<Elf> allElves)
    {
      List<Point> targetPoints = new();
      foreach (CardinalDirection checkDir in CheckDirections)
      {
        Point target = new Point(elfToMove.Location.X + checkDir.MoveOffsetX, elfToMove.Location.Y + checkDir.MoveOffsetY);
        targetPoints.Add(target);
      }

      foreach (Elf other in allElves.Where(elf => elf != elfToMove))
      {
        if (Math.Abs(other.Location.X - elfToMove.Location.X) > 1 || Math.Abs(other.Location.Y - elfToMove.Location.Y) > 1)
        {
          // The other elf is not close enough, so no need to check the points individually.
          continue;
        }
        foreach (Point targetPoint in targetPoints)
        {
          if (targetPoint == other.Location)
          {
            return false;
          }
        }
      }
      return true;
    }
  }
}
