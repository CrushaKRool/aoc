#undef DEBUG

using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace aoc22.Puzzles.Day18
{
  public class Day18 : IPuzzle
  {
    public string PuzzleName => "Day 18: Boiling Boulders";

    public string InputFileName => @"Input.txt";

    /// <summary>
    /// Maximum width, height and length of the 3D grid. Estimated by looking at the input coordinates.
    /// </summary>
    private const int GridSize = 22;

    // Some constants to distinguish different cell types in the grid.
    private const int DefaultAir = 0;
    private const int OutsideAir = 1;
    private const int InsideAir = 2;
    private const int Rock = 3;

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
      int[,,] grid = ParseGrid(input);
      int totalFaceCount = 0;

#if DEBUG
      for (int z = 0; z < GridSize; z++)
      {
        PrintLayer(grid, z);
        Console.WriteLine("--------------------------------------");
        Console.ReadKey(true);
      }
#endif

      for (int x = 0; x < GridSize; x++)
      {
        for (int y = 0; y < GridSize; y++)
        {
          for (int z = 0; z < GridSize; z++)
          {
            totalFaceCount += GetNumExposedFaces(grid, x, y, z, false);
          }
        }
      }

      Console.WriteLine($"{totalFaceCount} faces are exposed.");

      totalFaceCount = 0;

      for (int x = 0; x < GridSize; x++)
      {
        for (int y = 0; y < GridSize; y++)
        {
          for (int z = 0; z < GridSize; z++)
          {
            totalFaceCount += GetNumExposedFaces(grid, x, y, z, true);
          }
        }
      }

      Console.WriteLine($"{totalFaceCount} non-enclosed faces are exposed.");
    }

    /// <summary>
    /// Creates a 3D grid that has each input coordinate set to true.
    /// </summary>
    /// <param name="input">One set of 3D coordinates per line.</param>
    /// <returns>3D grid of int values. See constants at top of class for the possible values.</returns>
    private int[,,] ParseGrid(string input)
    {
      int[,,] grid = new int[GridSize, GridSize, GridSize];

      foreach (string inputLine in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)))
      {
        int[] coords = inputLine.Split(',').Select(s => int.Parse(s)).ToArray();
        grid[coords[0], coords[1], coords[2]] = Rock;
      }

      MarkEnclosedAir(grid);

      return grid;
    }

    /// <summary>
    /// Counts the number of rock faces in the grid that are not adjacent to another rock.
    /// </summary>
    /// <param name="grid">The grid to check.</param>
    /// <param name="x">X coordinate of the cell to check.</param>
    /// <param name="y">Y coordinate of the cell to check.</param>
    /// <param name="z">Z coordinate of the cell to check.</param>
    /// <param name="ignoreInsideFacing">Switches between part 1 and 2 logic. If true, does not count faces that meet enclosed air pockets.</param>
    /// <returns>Number of exposed rock faces.</returns>
    private int GetNumExposedFaces(int[,,] grid, int x, int y, int z, bool ignoreInsideFacing)
    {
      int result = 0;

      if (!RangeCheck(x, y, z) || grid[x, y, z] != Rock)
      {
        // Coordinate is not on the grid or not a filled in space.
        return result;
      }

      foreach (int[] offset in Offsets)
      {
        int xCheck = x + offset[0];
        int yCheck = y + offset[1];
        int zCheck = z + offset[2];
        if (ignoreInsideFacing)
        {
          if (!RangeCheck(xCheck, yCheck, zCheck) || grid[xCheck, yCheck, zCheck] == OutsideAir || grid[xCheck, yCheck, zCheck] == DefaultAir)
          {
            // Coordinate at the offset is outside air, so we have a face there.
            result++;
          }
        }
        else
        {
          if (!RangeCheck(xCheck, yCheck, zCheck) || grid[xCheck, yCheck, zCheck] != Rock)
          {
            // Coordinate at the offset is free, so we have a face there.
            result++;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// For part 2 we need to distinguish outside air from enclosed air pockets.
    /// This does that, assuming the rocks were already entered into the grid.
    /// </summary>
    /// <param name="grid">Grid with rocks.</param>
    private void MarkEnclosedAir(int[,,] grid)
    {
      for (int x = 0; x < GridSize; x++)
      {
        for (int y = 0; y < GridSize; y++)
        {
          for (int z = 0; z < GridSize; z++)
          {
            if (grid[x,y,z] == Rock)
            {
              continue;
            }
            if (ConnectsToOutside(grid, x, y, z))
            {
              grid[x, y, z] = OutsideAir;
            }
            else
            {
              grid[x, y, z] = InsideAir;
            }
          }
        }
      }
    }

    /// <summary>
    /// Performs a modified BFS to determine if the point at the given coordinates has a connection to the outside.
    /// </summary>
    /// <param name="grid">Grid.</param>
    /// <param name="x">X to check.</param>
    /// <param name="y">Y to check.</param>
    /// <param name="z">Z to check.</param>
    /// <returns></returns>
    private bool ConnectsToOutside(int[,,] grid, int x, int y, int z)
    {
      Queue<Vector3> q = new();
      ISet<Vector3> explored = new HashSet<Vector3>();
      Vector3 start = new(x, y, z);
      explored.Add(start);
      q.Enqueue(start);
      while (q.Count > 0)
      {
        Vector3 v = q.Dequeue();
        int vX = (int)v.X;
        int vY = (int)v.Y;
        int vZ = (int)v.Z;
        if (grid[vX, vY, vZ] == OutsideAir)
        {
          // We connect to something that was already determined to have an outside connection.
          return true;
        }
        foreach (int[] offset in Offsets)
        {
          int oX = vX + offset[0];
          int oY = vY + offset[1];
          int oZ = vZ + offset[2];
          if (!RangeCheck(oX, oY, oZ))
          {
            // We connect to something on the outside.
            return true;
          }
          if (grid[oX, oY, oZ] != Rock)
          {
            Vector3 neighbor = new(oX, oY, oZ);
            if (!explored.Contains(neighbor))
            {
              explored.Add(neighbor);
              q.Enqueue(neighbor);
            }
          }
        }
      }
      return false;
    }

    /// <summary>
    /// Checks whether the given coordinates are still within the defined grid range.
    /// </summary>
    private static bool RangeCheck(int x, int y, int z)
    {
      return x >= 0 && x < GridSize && y >= 0 && y < GridSize && z >= 0 && z < GridSize;
    }

    /// <summary>
    /// Visualizes a layer of the grid for debugging purposes.
    /// </summary>
    private static void PrintLayer(int[,,] grid, int zLayer)
    {
      for (int x = 0; x < GridSize; x++)
      {
        for (int y = 0; y < GridSize; y++)
        {
          char c = grid[x, y, zLayer] switch
          {
            DefaultAir => 'o',
            OutsideAir => '.',
            InsideAir => '/',
            Rock => '#',
            _ => throw new ArgumentException("Unknown grid cell type.")
          };
          Console.Write(c);
        }
        Console.WriteLine();
      }
    }
  }
}
