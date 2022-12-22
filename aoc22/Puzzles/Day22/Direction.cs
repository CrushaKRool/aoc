using System.Collections;
using System.Collections.Generic;

namespace aoc22.Puzzles.Day22
{
  internal sealed class Direction
  {
    public static Direction Right { get; } = new() { Value = 0, Symbol = '>', MoveOffsetX = 1 };
    public static Direction Down { get; } = new() { Value = 1, Symbol = 'v', MoveOffsetY = 1 };
    public static Direction Left { get; } = new() { Value = 2, Symbol = '<', MoveOffsetX = -1 };
    public static Direction Up { get; } = new() { Value = 3, Symbol = '^', MoveOffsetY = -1 };

    public static IDictionary<int, Direction> DirectionByValue { get; } = new Dictionary<int, Direction>()
    {
      { Right.Value, Right },
      { Down.Value, Down },
      { Left.Value, Left },
      { Up.Value, Up }
    };

    public static IDictionary<Direction, Direction> OppositeDirections { get; } = new Dictionary<Direction, Direction>()
    {
      { Right, Left },
      { Down, Up },
      { Left, Right },
      { Up, Down }
    };

    public int Value { get; init; }
    public char Symbol { get; init; }
    public int MoveOffsetX { get; init; }
    public int MoveOffsetY { get; init; }

    private Direction()
    {
      // static only
    }

    public override string ToString()
    {
      return Symbol.ToString();
    }
  }
}
