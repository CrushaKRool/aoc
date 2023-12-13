namespace aoc23.Puzzles.Day13
{
  internal class Pattern
  {
    public char[][] Data { get; }
    private int XMax { get; }
    private int YMax { get; }

    public Pattern(string inputPattern)
    {
      string[] lines = inputPattern.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Data = lines.Select(s => s.ToArray()).ToArray();
      XMax = lines[0].Length;
      YMax = lines.Length;
    }

    public int FindHorizontalMirrorIndex()
    {
      for (int x = 0; x < XMax - 1; x++)
      {
        if (HasHorizontalMirror(x))
        {
          return x + 1; // +1 to match the numbering in the puzzle.
        }
      }
      return -1;
    }

    /// <summary>
    /// Checks if there is a horizontal mirror for the data on each row between position X and X+1.
    /// </summary>
    private bool HasHorizontalMirror(int x)
    {
      for (int y = 0; y < YMax; y++)
      {
        if (!HasHorizontalMirror(x, y))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Checks if there is a horizontal mirror for the data in line Y between position X and X+1.
    /// </summary>
    private bool HasHorizontalMirror(int x, int y)
    {
      for (int i = 0; x - i >= 0 && x + 1 + i < XMax; i++)
      {
        bool isMirrored = Data[y][x - i] == Data[y][x + 1 + i];
        if (!isMirrored)
        {
          return false;
        }
      }
      return true;
    }

    public int FindVerticalMirrorIndex()
    {
      for (int y = 0; y < YMax - 1; y++)
      {
        if (HasVerticalMirror(y))
        {
          return y + 1; // +1 to match the numbering in the puzzle.
        }
      }
      return -1;
    }

    /// <summary>
    /// Checks if there is a vertical mirror for the data on each column between position Y and Y+1.
    /// </summary>
    private bool HasVerticalMirror(int y)
    {
      for (int x = 0; x < XMax; x++)
      {
        if (!HasVerticalMirror(x, y))
        {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    /// Checks if there is a vertical mirror for the data in column X between position Y and Y+1.
    /// </summary>
    private bool HasVerticalMirror(int x, int y)
    {
      for (int i = 0; y - i >= 0 && y + 1 + i < YMax; i++)
      {
        bool isMirrored = Data[y - i][x] == Data[y + 1 + i][x];
        if (!isMirrored)
        {
          return false;
        }
      }
      return true;
    }
  }
}
