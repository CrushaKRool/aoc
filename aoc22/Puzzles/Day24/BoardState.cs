using System.Collections.Generic;
using System.Drawing;

namespace aoc22.Puzzles.Day24
{
  internal class BoardState
  {
    public char[][] Grid { get; init; }
    public List<Blizzard> Blizzards { get; init; }

    public int Width { get => Grid[0].Length; }
    public int Height { get => Grid.Length; }

    public BoardState(char[][] grid, List<Blizzard> blizzards)
    {
      Grid = grid;
      Blizzards = blizzards;
    }

    public BoardState CalculateNextState()
    {
      // Copy current grid outline, with empty content.
      char[][] nextGrid = new char[Grid.Length][];
      for (int y = 0; y < Grid.Length; y++)
      {
        nextGrid[y] = new char[Grid[y].Length];
        for (int x = 0; x < Grid[y].Length; x++)
        {
          if (Grid[y][x] == '#')
          {
            nextGrid[y][x] = '#';
          }
          else
          {
            nextGrid[y][x] = '.';
          }
        }
      }

      List<Blizzard> nextBlizzards = new();
      foreach (Blizzard b in Blizzards)
      {
        int newX = b.X + b.MoveDir.MoveOffsetX;
        int newY = b.Y + b.MoveDir.MoveOffsetY;
        if (newX <= 0)
        {
          newX = Width - 2;
        }
        else if (newX >= Width - 1)
        {
          newX = 1;
        }
        if (newY <= 0)
        {
          newY = Height - 2;
        }
        else if (newY >= Height - 1)
        {
          newY = 1;
        }
        nextBlizzards.Add(new Blizzard() { X = newX, Y = newY, MoveDir = b.MoveDir });

        // Add blizzard to new grid.
        char nextGridChar = nextGrid[newY][newX];
        if (nextGridChar == '.')
        {
          nextGrid[newY][newX] = char.Parse(b.MoveDir.Symbol);
        }
        else if (char.IsDigit(nextGridChar))
        {
          int digit = int.Parse(nextGridChar.ToString());
          if (digit < 9)
          {
            digit++;
          }
          nextGrid[newY][newX] = char.Parse(digit.ToString());
        }
      }

      return new BoardState(nextGrid, nextBlizzards);
    }

    public bool CanMoveHere(int x, int y)
    {
      if (x < 0 || x >= Width || y < 0 || y >= Height)
      {
        return false;
      }
      return Grid[y][x] == '.';
    }
  }
}
