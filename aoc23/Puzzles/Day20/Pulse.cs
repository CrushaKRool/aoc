namespace aoc23.Puzzles.Day20
{
  internal record Pulse(string Source, string Destination, bool High)
  {
    public bool Low { get; } = !High;
  }
}
