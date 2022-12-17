using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace aoc22.Puzzles.Day17
{
  internal class Grid
  {
    public const int GridWidth = 7;

    public List<bool[]> Data = new();
    public long HighestRockRow { get; set; } = -1;

    public long ErasedHeight { get; set; } = 0;

    public bool[] GetDataRow(long rowIndex)
    {
      return Data[(int)(rowIndex - ErasedHeight)];
    }

    public void EnsureSpace(int freeRows)
    {
      while (Data.Count - ErasedHeight < HighestRockRow + freeRows)
      {
        Data.Add(new bool[GridWidth]);
      }

      // Prune rows that are below a fully filled row if we grow too steep.
      if (Data.Count > 1030)
      {
        int highestFilledRow = -1;
        for (int y = 0; y < Data.Count - 30; y++)
        {
          if (Data[y].All(b => b))
          {
            highestFilledRow = y;
          }
        }
        if (highestFilledRow > 0)
        {
          Data.RemoveRange(0, highestFilledRow);
          ErasedHeight += highestFilledRow;
        }
      }
    }

    public void FixateRock(Rock rock)
    {
      foreach (Point p in rock.Offsets)
      {
        GetDataRow(rock.Y + p.Y)[rock.X + p.X] = true;
      }
      // From the current highest row upwards, check if any of the higher rows contain a rock part now.
      for (long y = HighestRockRow; y < Data.Count; y++)
      {
        if (y - ErasedHeight < 0)
        {
          continue;
        }
        if (GetDataRow(y).Any(b => b))
        {
          HighestRockRow = y;
        }
      }
    }

    public void PrintGridSection(long startHeight, long endHeight, Rock? curRock)
    {
      if (startHeight > endHeight)
      {
        (startHeight, endHeight) = (endHeight, startHeight);
      }
      for (long y = endHeight; y > startHeight; y--)
      {
        if (y - ErasedHeight < 0)
        {
          break;
        }
        if (y - ErasedHeight >= Data.Count)
        {
          continue;
        }
        Console.Write('|');
        for (int x = 0; x < GridWidth; x++)
        {
          if (GetDataRow(y)[x])
          {
            Console.Write('#');
          }
          else if (curRock != null && curRock.OverlapsPoint(x, y))
          {
            Console.Write('@');
          }
          else
          {
            Console.Write('.');
          }
        }
        Console.WriteLine('|');
      }
      if (startHeight <= 0)
      {
        Console.WriteLine("+-------+");
      }
      Console.WriteLine();
    }
  }
}
