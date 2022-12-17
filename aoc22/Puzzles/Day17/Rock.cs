using System.Drawing;

namespace aoc22.Puzzles.Day17
{
  internal class Rock
  {
    public int X { get; set; } // This will never be longer than 7, so int is fine.
    public long Y { get; set; } // This can grow really tall when remembering the real coordinates. But we will never access an array index that tall.

    public Point[] Offsets { get; }

    public Rock(params Point[] offsets)
    {
      Offsets = offsets;
    }

    public bool CanFall(Grid grid)
    {
      // Optimization: If we are above the highest row, there is always room to fall.
      if (Y > grid.HighestRockRow + 1)
      {
        return true;
      }
      foreach (Point p in Offsets)
      {
        // Check if coordinate below the point is already taken.
        int newX = X + p.X;
        long newY = Y + p.Y - 1;
        if (newY - grid.ErasedHeight < 0)
        {
          return false;
        }
        if (grid.GetDataRow(newY)[newX])
        {
          return false;
        }
      }
      return true;
    }

    public bool CanMoveSideways(Grid grid, int desiredOffset)
    {
      foreach (Point p in Offsets)
      {
        // Check if coordinate in the offset direction is already taken.
        int newX = X + p.X + desiredOffset;
        long newY = Y + p.Y;
        if (newX < 0 || newX >= Grid.GridWidth)
        {
          return false;
        }
        if (grid.GetDataRow(newY)[newX])
        {
          return false;
        }
      }
      return true;
    }

    public bool OverlapsPoint(int x, long y)
    {
      foreach (Point p in Offsets)
      {
        if (X + p.X == x && Y + p.Y == y)
        {
          return true;
        }
      }
      return false;
    }
  }
}
