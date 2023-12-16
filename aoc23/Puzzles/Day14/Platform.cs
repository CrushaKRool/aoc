using aoc_common;

namespace aoc23.Puzzles.Day14
{
  internal class Platform(string input) : Grid(input)
  {
    public void TiltNorth()
    {
      bool changed;
      do
      {
        changed = MoveAllNorth();
      }
      while (changed);
    }

    public void TiltWest()
    {
      bool changed;
      do
      {
        changed = MoveAllWest();
      }
      while (changed);
    }

    public void TiltSouth()
    {
      bool changed;
      do
      {
        changed = MoveAllSouth();
      }
      while (changed);
    }

    public void TiltEast()
    {
      bool changed;
      do
      {
        changed = MoveAllEast();
      }
      while (changed);
    }

    /// <summary>
    /// Moves all loose rocks 1 step north, if possible.
    /// </summary>
    /// <returns>True if any rocks moved this iteration.</returns>
    private bool MoveAllNorth()
    {
      bool changed = false;
      for (int y = 1; y < YMax; y++)
      {
        for (int x = 0; x < XMax; x++)
        {
          char oldPos = Data[y][x];
          if (oldPos != 'O')
          {
            continue;
          }
          char newPos = Data[y - 1][x];
          if (newPos != '.')
          {
            continue;
          }
          Data[y - 1][x] = oldPos;
          Data[y][x] = '.';
          changed = true;
        }
      }
      return changed;
    }

    /// <summary>
    /// Moves all loose rocks 1 step west, if possible.
    /// </summary>
    /// <returns>True if any rocks moved this iteration.</returns>
    private bool MoveAllWest()
    {
      bool changed = false;
      for (int x = 1; x < XMax; x++)
      {
        for (int y = 0; y < YMax; y++)
        {
          char oldPos = Data[y][x];
          if (oldPos != 'O')
          {
            continue;
          }
          char newPos = Data[y][x - 1];
          if (newPos != '.')
          {
            continue;
          }
          Data[y][x - 1] = oldPos;
          Data[y][x] = '.';
          changed = true;
        }
      }
      return changed;
    }

    /// <summary>
    /// Moves all loose rocks 1 step south, if possible.
    /// </summary>
    /// <returns>True if any rocks moved this iteration.</returns>
    private bool MoveAllSouth()
    {
      bool changed = false;
      for (int y = YMax - 2; y >= 0; y--)
      {
        for (int x = 0; x < XMax; x++)
        {
          char oldPos = Data[y][x];
          if (oldPos != 'O')
          {
            continue;
          }
          char newPos = Data[y + 1][x];
          if (newPos != '.')
          {
            continue;
          }
          Data[y + 1][x] = oldPos;
          Data[y][x] = '.';
          changed = true;
        }
      }
      return changed;
    }

    /// <summary>
    /// Moves all loose rocks 1 step east, if possible.
    /// </summary>
    /// <returns>True if any rocks moved this iteration.</returns>
    private bool MoveAllEast()
    {
      bool changed = false;
      for (int x = XMax - 2; x >= 0; x--)
      {
        for (int y = 0; y < YMax; y++)
        {
          char oldPos = Data[y][x];
          if (oldPos != 'O')
          {
            continue;
          }
          char newPos = Data[y][x + 1];
          if (newPos != '.')
          {
            continue;
          }
          Data[y][x + 1] = oldPos;
          Data[y][x] = '.';
          changed = true;
        }
      }
      return changed;
    }

    public long MeasureNorthLoad()
    {
      long load = 0;
      for (int y = 0; y < YMax; y++)
      {
        for (int x = 0; x < XMax; x++)
        {
          char pos = Data[y][x];
          if (pos == 'O')
          {
            load += YMax - y;
          }
        }
      }
      return load;
    }
  }
}
