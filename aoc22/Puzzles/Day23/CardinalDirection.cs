using System.Collections;
using System.Collections.Generic;

namespace aoc22.Puzzles.Day23
{
  internal sealed class CardinalDirection
  {
    public static CardinalDirection N { get; } = new() { Symbol = "N", MoveOffsetY = -1 };
    public static CardinalDirection NE { get; } = new() { Symbol = "NE", MoveOffsetX = 1, MoveOffsetY = -1 };
    public static CardinalDirection E { get; } = new() { Symbol = "E", MoveOffsetX = 1 };
    public static CardinalDirection SE { get; } = new() { Symbol = "SE", MoveOffsetX = 1, MoveOffsetY = 1 };
    public static CardinalDirection S { get; } = new() { Symbol = "S", MoveOffsetY = 1 };
    public static CardinalDirection SW { get; } = new() { Symbol = "SW", MoveOffsetX = -1, MoveOffsetY = 1 };
    public static CardinalDirection W { get; } = new() { Symbol = "W", MoveOffsetX = -1 };
    public static CardinalDirection NW { get; } = new() { Symbol = "NW", MoveOffsetX = -1, MoveOffsetY = -1 };

    public string Symbol { get; init; }
    public int MoveOffsetX { get; init; }
    public int MoveOffsetY { get; init; }

    private CardinalDirection()
    {
      Symbol = "";
    }

    public override string ToString()
    {
      return Symbol.ToString();
    }
  }
}
