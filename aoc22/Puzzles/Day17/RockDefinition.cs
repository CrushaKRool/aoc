using System.Drawing;

namespace aoc22.Puzzles.Day17
{
  internal class RockDefinition
  {
    public Point[] Offsets { get; }

    public RockDefinition(params Point[] offsets)
    {
      Offsets = offsets;
    }

    public Rock SpawnRock(int startX, long startY)
    {
      return new Rock(Offsets) { X = startX, Y = startY };
    }
  }
}
