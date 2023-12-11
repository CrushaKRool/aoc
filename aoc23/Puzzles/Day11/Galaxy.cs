namespace aoc23.Puzzles.Day11
{
  internal class Galaxy(long x, long y)
  {
    public long X { get; set; } = x;
    public long Y { get; set; } = y;

    public long DistanceToOther(Galaxy other)
    {
      return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    }
  }
}
