using System.Drawing;

namespace aoc23.Puzzles.Day03
{
  internal class Grid
  {
    private readonly char[][] Data;

    public Grid(string input)
    {
      List<char[]> gridLines = [];
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        gridLines.Add(inputLine.ToCharArray());
      }
      Data = [.. gridLines];
    }

    public List<GridNumber> FindGridNumbers()
    {
      GridNumber? curNum = null;
      List<GridNumber> result = [];
      for (int y = 0; y < Data.Length; y++)
      {
        for (int x = 0; x < Data[y].Length; x++)
        {
          char curChar = Data[y][x];
          if (char.IsDigit(curChar))
          {
            curNum ??= new()
            {
              X = x,
              Y = y
            };
            curNum.Digits.Append(curChar);
          }
          else if (curNum != null)
          {
            result.Add(curNum);
            curNum = null;
          }
        }
        if (curNum != null)
        {
          result.Add(curNum);
          curNum = null;
        }
      }
      return result;
    }

    public List<Point> FindGears()
    {
      List<Point> result = [];
      for (int y = 0; y < Data.Length; y++)
      {
        for (int x = 0; x < Data[y].Length; x++)
        {
          char curChar = Data[y][x];
          if (curChar == '*')
          {
            result.Add(new Point(x, y));
          }
        }
      }
      return result;
    }

    public bool IsNumberAdjacentToSymbol(GridNumber number)
    {
      return number.GetSurroundingPoints()
        .Where(PointInGrid)
        .Any(p => IsSymbol(Data[p.Y][p.X]));
    }

    private bool PointInGrid(Point point)
    {
      return point.Y >= 0 && point.Y < Data.Length
        && point.X >= 0 && point.X < Data[point.Y].Length;
    }

    private static bool IsSymbol(char c)
    {
      return !char.IsDigit(c) && c != '.';
    }
  }
}
