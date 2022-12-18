using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day18
{
  public class Day18 : IPuzzle
  {
    public string PuzzleName => "Day 18: Boiling Boulders";

    public string InputFileName => @"Puzzles\Day18\Day18Input.txt";

    /// <summary>
    /// Maximum width, height and length of the 3D grid. Estimated by looking at the input coordinates.
    /// </summary>
    private const int GridSize = 22;

    /// <summary>
    /// Defines the adjacent coordinates to check for any given cube.
    /// </summary>
    private readonly List<int[]> Offsets = new()
    {
      new int[] {-1,0,0},
      new int[] {1,0,0},
      new int[] {0,-1,0},
      new int[] {0,1,0},
      new int[] {0,0,-1},
      new int[] {0,0,1},
    };

    public void Run(string input)
    {
      bool[,,] grid = ParseGrid(input);
      int totalFaceCount = 0;

      for (int x = 0; x < GridSize; x++)
      {
        for (int y = 0; y < GridSize; y++)
        {
          for (int z = 0; z < GridSize; z++)
          {
            totalFaceCount += GetNumExposedFaces(grid, x, y, z);
          }
        }
      }

      Console.WriteLine($"{totalFaceCount} faces are exposed.");
    }

    /// <summary>
    /// Creates a 3D grid that has each input coordinate set to true.
    /// </summary>
    /// <param name="input">One set of 3D coordinates per line.</param>
    /// <returns>3D grid of boolean values.</returns>
    private bool[,,] ParseGrid(string input)
    {
      bool[,,] grid = new bool[GridSize, GridSize, GridSize];

      foreach (string inputLine in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)))
      {
        int[] coords = inputLine.Split(',').Select(s => int.Parse(s)).ToArray();
        grid[coords[0], coords[1], coords[2]] = true;
      }

      return grid;
    }

    private int GetNumExposedFaces(bool[,,] grid, int x, int y, int z)
    {
      int result = 0;

      if (!RangeCheck(x, y, z) || !grid[x, y, z])
      {
        // Coordinate is not on the grid or not a filled in space.
        return result;
      }

      foreach (int[] offset in Offsets)
      {
        int xCheck = x + offset[0];
        int yCheck = y + offset[1];
        int zCheck = z + offset[2];
        if (!RangeCheck(xCheck, yCheck, zCheck) || !grid[xCheck, yCheck, zCheck])
        {
          // Coordinate at the offset is free, so we have a face there.
          result++;
        }
      }

      return result;
    }

    private bool RangeCheck(int x, int y, int z)
    {
      return x >= 0 && x < GridSize && y >= 0 && y < GridSize && z >= 0 && z < GridSize;
    }
  }
}
