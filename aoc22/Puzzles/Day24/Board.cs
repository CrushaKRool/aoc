using System;
using System.Collections.Generic;

namespace aoc22.Puzzles.Day24
{
  internal class Board
  {
    public IDictionary<int, BoardState> BoardStates { get; } = new Dictionary<int, BoardState>();

    public int Width { get => BoardStates[0].Width; }
    public int Height { get => BoardStates[0].Height; }

    public Board(string input)
    {
      int y = 0;
      List<char[]> grid = new();
      List<Blizzard> blizzards = new();
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        grid.Add(inputLine.ToCharArray());
        for (int x = 0; x < inputLine.Length; x++)
        {
          char c = inputLine[x];
          switch (c)
          {
            case '^':
              blizzards.Add(new Blizzard() { X = x, Y = y, MoveDir = CardinalDirection.N });
              break;
            case '>':
              blizzards.Add(new Blizzard() { X = x, Y = y, MoveDir = CardinalDirection.E });
              break;
            case 'v':
              blizzards.Add(new Blizzard() { X = x, Y = y, MoveDir = CardinalDirection.S });
              break;
            case '<':
              blizzards.Add(new Blizzard() { X = x, Y = y, MoveDir = CardinalDirection.W });
              break;
            default:
              break;
          }
        }
        y++;
      }
      BoardStates[0] = new BoardState(grid.ToArray(), blizzards);

      PrecomputeStates();
    }

    private void PrecomputeStates()
    {
      int maxX = BoardStates[0].Grid[0].Length;
      int maxY = BoardStates[0].Grid.Length;
      int maxCycle = (maxX - 2) * (maxY - 2);
      for (int i = 1; i < maxCycle; i++)
      {
        BoardStates[i] = BoardStates[i - 1].CalculateNextState();
      }
    }

    /// <summary>
    /// Performs a BFS to find the shortest path to between start and end.
    /// </summary>
    /// <param name="startX">Start X coordinate.</param>
    /// <param name="startY">Start Y coordinate.</param>
    /// <param name="endX">End X coordinate.</param>
    /// <param name="endY">End Y coordinate.</param>
    /// <param name="startTick">Current tick during the first round.</param>
    /// <returns>End node, if found. Each node contains a reference to its predecessor, so the full path can be obtained.</returns>
    public PositionState? FindShortestPath(int startX, int startY, int endX, int endY, int startTick)
    {
      Queue<PositionState> queue = new();
      HashSet<PositionState> explored = new();

      PositionState root = new(startTick, startX, startY);
      explored.Add(root);
      queue.Enqueue(root);

      while (queue.Count > 0)
      {
        PositionState ps = queue.Dequeue();
        if (ps.X == endX && ps.Y == endY)
        {
          return ps;
        }
        List<PositionState> nextCandidates = new()
        {
          new(ps.Tick + 1, ps.X, ps.Y - 1, ps), // North.
          new(ps.Tick + 1, ps.X + 1, ps.Y, ps), // East.
          new(ps.Tick + 1, ps.X, ps.Y + 1, ps), // South.
          new(ps.Tick + 1, ps.X - 1, ps.Y, ps), // West.
          new(ps.Tick + 1, ps.X, ps.Y, ps), // Waiting here.
        };

        BoardState nextBoard = GetBoardStateForTick(ps.Tick + 1);

        foreach (PositionState candidate in nextCandidates)
        {
          if (explored.Contains(candidate))
          {
            continue;
          }
          if (nextBoard.CanMoveHere(candidate.X, candidate.Y))
          {
            explored.Add(candidate);
            queue.Enqueue(candidate);
          }
        }
      }
      return null;
    }

    private BoardState GetBoardStateForTick(int tick)
    {
      return BoardStates[tick % BoardStates.Count];
    }

    public void PrintPositionState(PositionState state)
    {
      BoardState bs = GetBoardStateForTick(state.Tick);
      for (int y = 0; y < bs.Height; y++)
      {
        for (int x = 0; x < bs.Width; x++)
        {
          if (state.X == x && state.Y == y)
          {
            Console.Write('E');
          }
          else
          {
            char c = bs.Grid[y][x];
            if (c == '#' || c == '.')
            {
              Console.Write(c);
            }
            else
            {
              Console.ForegroundColor = c switch
              {
                'N' => ConsoleColor.Red,
                'E' => ConsoleColor.Magenta,
                'S' => ConsoleColor.Green,
                'W' => ConsoleColor.Blue,
                _ => ConsoleColor.White,
              };
              Console.Write(c);
              Console.ResetColor();
            }
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}
