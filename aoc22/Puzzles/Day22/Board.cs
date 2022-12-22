using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day22
{
  internal class Board
  {
    public char[][] Grid { get; init; }

    public int PlayerX { get; set; }
    public int PlayerY { get; set; }
    public Direction PlayerFacing { get; set; } = Direction.Right;

    public Board(IEnumerable<string> inputLines)
    {
      List<char[]> gridCollector = new();
      foreach (string inputLine in inputLines)
      {
        gridCollector.Add(inputLine.ToCharArray());
      }
      Grid = gridCollector.ToArray();
    }

    public void RotatePlayer(char turnDir)
    {
      PlayerFacing = turnDir switch
      {
        'R' => Direction.DirectionByValue[(PlayerFacing.Value + 1) % 4],
        'L' => Direction.DirectionByValue[(PlayerFacing.Value + 3) % 4], // -1 + 4, to fix the modulo for negative numbers.
        _ => throw new ArgumentException($"Unknown rotation direction: {turnDir}"),
      };
    }

    public void MovePlayer(int steps)
    {
      for (int i = 0; i < steps; i++)
      {
        int newX = PlayerX + PlayerFacing.MoveOffsetX;
        int newY = PlayerY + PlayerFacing.MoveOffsetY;
        if (IsPointOffGrid(newX, newY))
        {
          FindWrapAroundDestination(newX, newY, out newX, out newY);
        }
        if (IsPointBlocked(newX, newY))
        {
          break;
        }
        PlayerX = newX;
        PlayerY = newY;
      }
    }

    private void FindWrapAroundDestination(int newX, int newY, out int wrapX, out int wrapY)
    {
      Direction opposite = Direction.OppositeDirections[PlayerFacing];
      do
      {
        wrapX = newX;
        wrapY = newY;
        newX += opposite.MoveOffsetX;
        newY += opposite.MoveOffsetY;
      } while (!IsPointOffGrid(newX, newY));
    }

    private bool IsPointOffGrid(int x, int y)
    {
      return y < 0 || y >= Grid.Length
        || x < 0 || x >= Grid[y].Length
        || Grid[y][x] == ' ';
    }

    private bool IsPointBlocked(int x, int y)
    {
      return Grid[y][x] == '#';
    }

    public void PrintBoard()
    {
      for (int y = 0; y < Grid.Length; y++)
      {
        for (int x = 0; x < Grid[y].Length; x++)
        {
          if (x == PlayerX && y == PlayerY)
          {
            Console.Write(PlayerFacing);
          }
          else
          {
            Console.Write(Grid[y][x]);
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}
