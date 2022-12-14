using System;
using System.Collections.Generic;
using System.Drawing;

namespace aoc22.Puzzles.Day14
{
  internal class RockLine
  {
    public List<Point> RockPoints { get; set; } = new();

    public RockLine(string inputLine)
    {
      foreach (string coords in inputLine.Split(" -> "))
      {
        RockPoints.Add(ParseCoords(coords));
      }
    }

    private static Point ParseCoords(string coords)
    {
      string[] coordPair = coords.Split(",");
      int x = int.Parse(coordPair[0]);
      int y = int.Parse(coordPair[1]);
      return new Point(x, y);
    }

    public void FillRocksInGrid(char[,] grid)
    {
      for (int i = 0; i < RockPoints.Count - 1; i++)
      {
        Point current = RockPoints[i];
        Point next = RockPoints[i + 1];

        if (current.X == next.X)
        {
          int x = current.X;
          int y = current.Y;
          int targetY = next.Y;
          do
          {
            grid[x,y] = '#';
            y += Math.Sign(next.Y - current.Y);
          }
          while (y != targetY);
          grid[x, y] = '#';
        }
        else if (current.Y == next.Y)
        {
          int x = current.X;
          int y = current.Y;
          int targetX = next.X;
          do
          {
            grid[x,y] = '#';
            x += Math.Sign(next.X - current.X);
          }
          while (x != targetX);
          grid[x, y] = '#';
        }
        else
        {
          throw new ArgumentException($"Unable to find straight line between {current} and {next}.");
        }
      }
    }
  }
}
