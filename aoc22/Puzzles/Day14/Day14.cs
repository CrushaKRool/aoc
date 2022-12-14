using aoc_common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace aoc22.Puzzles.Day14
{
  public class Day14 : IPuzzle
  {
    /// <summary>
    /// Output current state every N steps.
    /// </summary>
    private const int DebugStepSize = 3000;

    // We could calculate the maximum bounds from the input...
    // ... or we just look at it with our eyes and conclude that (1000,200) is enough to fit everything, with (500,0) being the sand source.
    private const int MaxWidth = 750;
    private const int MaxHeight = 200;

    // Cut this much off from the start of the grid when visualizing it, since it won't contain anything useful.
    private const int CullX = 450;
    private const int CullY = 0;

    public string PuzzleName => "Day 14: Regolith Reservoir";

    public string InputFileName => @"Puzzles\Day14\Day14Input.txt";

    public void Run(string input)
    {
      char[,] grid = CreateGrid(input);

      bool sandCameToRest = true;
      int i = 0;
      while (sandCameToRest && grid[500, 0] == '+')
      {
        sandCameToRest = GenerateSand(500, 0, grid);
        i++;

        // Debug output once every few steps.
        if (i % DebugStepSize == 0)
        {
          Console.WriteLine("Press any key to continue");
          Console.ReadKey(true);
          PrintGrid(grid);
        }
      }

      PrintGrid(grid);

      Console.WriteLine($"A sand grain fell into the void.");

      // Part 2

      grid = CreateGrid(input);

      int lowestRock = FindLowestRock(grid);
      for (int x = 0; x < MaxWidth; x++)
      {
        grid[x, lowestRock + 2] = '#';
      }

      sandCameToRest = true;
      i = 0;
      while (sandCameToRest && grid[500, 0] == '+')
      {
        sandCameToRest = GenerateSand(500, 0, grid);
        i++;

        // Debug output once every few steps.
        if (i % DebugStepSize == 0)
        {
          Console.WriteLine("Press any key to continue");
          Console.ReadKey(true);
          PrintGrid(grid);
        }
      }

      PrintGrid(grid);

      Console.WriteLine($"The source was suffocated.");
    }

    private static char[,] CreateGrid(string input)
    {
      List<RockLine> rockLines = new();
      foreach (string inputLine in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)).Distinct())
      {
        rockLines.Add(new RockLine(inputLine));
      }

      // We could calculate the maximum bounds from the input...
      // ... or we just look at it with our eyes and conclude that (1000,200) is enough to fit everything, with (500,0) being the sand source.
      char[,] grid = new char[MaxWidth, MaxHeight];

      // Init all with empty space, so it's visible in the console.
      for (int y = 0; y < MaxHeight; y++)
      {
        for (int x = 0; x < MaxWidth; x++)
        {
          grid[x, y] = '.';
        }
      }

      foreach (RockLine rockLine in rockLines)
      {
        rockLine.FillRocksInGrid(grid);
      }

      // Sand source.
      grid[500, 0] = '+';
      return grid;
    }

    /// <summary>
    /// Attempts to spawn a new grain of sand and moves it along the grid until it rests.
    /// </summary>
    /// <param name="start">Starting point from where to spawn the sand.</param>
    /// <param name="grid">The grid where to place the grain.</param>
    /// <returns>True, if the grain came to rest. False, if it fell into the void.</returns>
    private bool GenerateSand(int startX, int startY, char[,] grid)
    {
      int x = startX;
      int y = startY;

      do
      {
        if (TryMoveToPosition(grid, ref x, ref y, 0, 1)
          || TryMoveToPosition(grid, ref x, ref y, -1, 1)
          || TryMoveToPosition(grid, ref x, ref y, 1, 1))
        {
          if (IsInVoid(x, y))
          {
            return false;
          }
          continue;
        }
        grid[x, y] = 'o';
        return true;
      }
      while (true);
    }

    private bool TryMoveToPosition(char[,] grid, ref int curX, ref int curY, int deltaX, int deltaY)
    {
      bool canMove;
      int newX = curX + deltaX;
      int newY = curY + deltaY;
      if (IsInVoid(newX, newY))
      {
        canMove = true;
      }
      else
      {
        canMove = grid[newX, newY] == '.';
      }
      if (canMove)
      {
        curX += deltaX;
        curY += deltaY;
      }
      return canMove;
    }

    private bool IsInVoid(int x, int y)
    {
      return x < 0 || x >= MaxWidth || y < 0 || y >= MaxHeight;
    }

    private int FindLowestRock(char[,] grid)
    {
      for (int y = MaxHeight - 1; y >= 0; y--)
      {
        for (int x = 0; x < MaxWidth; x++)
        {
          if (grid[x, y] == '#')
          {
            return y;
          }
        }
      }
      return -1;
    }

    private static void PrintGrid(char[,] grid)
    {
      for (int y = CullY; y < MaxHeight; y++)
      {
        for (int x = CullX; x < MaxWidth; x++)
        {
          Console.Write(grid[x, y]);
        }
        Console.WriteLine();
      }
      int numSand = 0;
      for (int y = 0; y < MaxHeight; y++)
      {
        for (int x = 0; x < MaxWidth; x++)
        {
          if (grid[x, y] == 'o')
          {
            numSand++;
          }
        }
      }
      Console.WriteLine($"{numSand} grains of sand have come to rest.");
    }
  }
}