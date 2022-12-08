using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles
{
  class Day8 : IPuzzleSolver
  {
    private const int UninitalizedTreeHeight = -1;

    public string PuzzleName => "Day 8: Treetop Tree House";

    private int sizeX;
    private int sizeY;

    public string SolvePart1(string input)
    {
      int[][] treeGrid = ParseTreeGrid(input);
      sizeX = treeGrid.Length;
      sizeY = treeGrid[0].Length;

      bool[,] visibleGrid = new bool[sizeX,sizeY];

      // Check all trees from the top.
      for(int x = 0; x < sizeX; x++)
      {
        int highestTree = UninitalizedTreeHeight;
        for (int y = 0; y < sizeY; y++)
        {
          CheckTreeVisibleAtIndex(x, y, treeGrid, visibleGrid, ref highestTree);
        }
      }

      // Check all trees from the bottom.
      for (int x = 0; x < sizeX; x++)
      {
        int highestTree = UninitalizedTreeHeight;
        for (int y = sizeY - 1; y >= 0; y--)
        {
          CheckTreeVisibleAtIndex(x, y, treeGrid, visibleGrid, ref highestTree);
        }
      }

      // Check all trees from the left.
      for (int y = 0; y < sizeY; y++)
      {
        int highestTree = UninitalizedTreeHeight;
        for (int x = 0; x < sizeX; x++)
        {
          CheckTreeVisibleAtIndex(x, y, treeGrid, visibleGrid, ref highestTree);
        }
      }

      // Check all trees from the right.
      for (int y = 0; y < sizeY; y++)
      {
        int highestTree = UninitalizedTreeHeight;
        for (int x = sizeX - 1; x >= 0; x--)
        {
          CheckTreeVisibleAtIndex(x, y, treeGrid, visibleGrid, ref highestTree);
        }
      }

      int visibleTreeCount = 0;
      for (int x = 0; x < sizeX; x++)
      {
        for (int y = 0; y < sizeY; y++)
        {
          if (visibleGrid[x,y])
          {
            visibleTreeCount++;
          }
        }
      }

      return $"{visibleTreeCount} trees are visible from the outside.";
    }

    public string SolvePart2(string input)
    {
      int[][] treeGrid = ParseTreeGrid(input);
      sizeX = treeGrid.Length;
      sizeY = treeGrid[0].Length;

      int bestScenicScore = -1;
      for (int x = 0; x < sizeX; x++)
      {
        for (int y = 0; y < sizeY; y++)
        {
          int scenicScore = GetScenicScore(x, y, treeGrid);
          if (scenicScore > bestScenicScore)
          {
            bestScenicScore = scenicScore;
          }
        }
      }

      return $"The best scenic score possible is {bestScenicScore}.";
    }

    private int[][] ParseTreeGrid(string input)
    {
      List<int[]> treeLines = new();
      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        int[] treeLine = line.Select(c => int.Parse(char.ToString(c))).ToArray();
        treeLines.Add(treeLine);
      }
      return treeLines.ToArray();
    }

    /// <summary>
    /// Checks if the tree at the given coordinates is higher than any tree seen so far (on the current line).
    /// If it is higher, it will be marked as visible on the grid.
    /// </summary>
    /// <param name="x">X coordinate of the tree to check.</param>
    /// <param name="y">Y coordinate of the tree to check.</param>
    /// <param name="treeGrid">Grid of all tree heights.</param>
    /// <param name="visibleGrid">Grid marking visible trees.</param>
    /// <param name="maxHeightSeen">Highest tree height seen on the current line. The function will set this parameter if the current tree is higher than this.</param>
    private void CheckTreeVisibleAtIndex(int x, int y, int[][] treeGrid, bool[,] visibleGrid, ref int maxHeightSeen)
    {
      int newTreeHeight = treeGrid[x][y];
      if (newTreeHeight > maxHeightSeen)
      {
        visibleGrid[x, y] = true;
        maxHeightSeen = newTreeHeight;
      }
    }

    private int GetScenicScore(int x, int y, int[][] treeGrid)
    {
      return GetViewDistanceNorth(x, y, treeGrid)
        * GetViewDistanceSouth(x, y, treeGrid)
        * GetViewDistanceEast(x, y, treeGrid)
        * GetViewDistanceWest(x, y, treeGrid);
    }

    private int GetViewDistanceNorth(int startX, int startY, int[][] treeGrid)
    {
      int startHeight = treeGrid[startX][startY];
      int viewDistance = 0;
      for (int y = startY - 1; y >= 0; y--)
      {
        viewDistance++;
        int checkHeight = treeGrid[startX][y];
        if (checkHeight >= startHeight)
        {
          return viewDistance;
        }
      }
      return viewDistance;
    }

    private int GetViewDistanceSouth(int startX, int startY, int[][] treeGrid)
    {
      int startHeight = treeGrid[startX][startY];
      int viewDistance = 0;
      for (int y = startY + 1; y < sizeY; y++)
      {
        viewDistance++;
        int checkHeight = treeGrid[startX][y];
        if (checkHeight >= startHeight)
        {
          return viewDistance;
        }
      }
      return viewDistance;
    }

    private int GetViewDistanceEast(int startX, int startY, int[][] treeGrid)
    {
      int startHeight = treeGrid[startX][startY];
      int viewDistance = 0;
      for (int x = startX + 1; x < sizeX; x++)
      {
        viewDistance++;
        int checkHeight = treeGrid[x][startY];
        if (checkHeight >= startHeight)
        {
          return viewDistance;
        }
      }
      return viewDistance;
    }

    private int GetViewDistanceWest(int startX, int startY, int[][] treeGrid)
    {
      int startHeight = treeGrid[startX][startY];
      int viewDistance = 0;
      for (int x = startX - 1; x >= 0; x--)
      {
        viewDistance++;
        int checkHeight = treeGrid[x][startY];
        if (checkHeight >= startHeight)
        {
          return viewDistance;
        }
      }
      return viewDistance;
    }
  }
}
