using System.Drawing;
using System.Text;

namespace aoc23.Puzzles.Day03
{
  internal class GridNumber
  {
    public StringBuilder Digits { get; } = new();
    public int X { get; set; }
    public int Y { get; set; }

    public int NumericValue { get { return int.Parse(Digits.ToString()); } }

    public IEnumerable<Point> GetSurroundingPoints()
    {
      int xStart = X - 1;
      int xEnd = X + Digits.Length;
      for (int x = xStart; x <= xEnd; x++)
      {
        yield return new Point(x, Y - 1);
      }
      yield return new Point(xStart, Y);
      yield return new Point(xEnd, Y);
      for (int x = xStart; x <= xEnd; x++)
      {
        yield return new Point(x, Y + 1);
      }
    }
  }
}
