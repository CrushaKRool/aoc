using System;

namespace aoc22.Puzzles.Day24
{
  internal class PositionState
  {
    public PositionState? Parent { get; init; }

    public int X { get; init; }
    public int Y { get; init; }
    public int Tick { get; init; }

    public PositionState(int tick , int x, int y)
    {
      Tick = tick;
      X = x;
      Y = y;
    }

    public PositionState(int tick, int x, int y, PositionState parent)
    {
      Tick = tick;
      X = x;
      Y = y;
      Parent = parent;
    }

    public override bool Equals(object? obj)
    {
      return obj is PositionState state &&
             X == state.X &&
             Y == state.Y &&
             Tick == state.Tick;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y, Tick);
    }
  }
}
