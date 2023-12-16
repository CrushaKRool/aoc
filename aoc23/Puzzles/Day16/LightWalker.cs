using System.Net;

namespace aoc23.Puzzles.Day16
{
  internal record LightWalker(int StartX, int StartY, EDirection StartDirection)
  {
    public int X { get; set; } = StartX;
    public int Y { get; set; } = StartY;
    public EDirection Direction { get; set; } = StartDirection;

    public EDirection OppositeDirection {
      get
      {
        return Direction switch
        {
          EDirection.UP => EDirection.DOWN,
          EDirection.DOWN => EDirection.UP,
          EDirection.LEFT => EDirection.RIGHT,
          EDirection.RIGHT => EDirection.LEFT,
          _ => throw new NotImplementedException(),
        };
      }
    }
  }
}
