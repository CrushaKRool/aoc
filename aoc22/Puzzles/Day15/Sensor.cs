using System;
using System.Collections.Generic;
using System.Drawing;

namespace aoc22.Puzzles.Day15
{
  internal class Sensor
  {
    public int X { get; }
    public int Y { get; }

    public int ClosestBeaconX { get; }
    public int ClosestBeaconY { get; }

    public int ManhattenDistance { get; }

    public Sensor(int x, int y, int closestBeaconX, int closestBeaconY)
    {
      X = x;
      Y = y;
      ClosestBeaconX = closestBeaconX;
      ClosestBeaconY = closestBeaconY;
      ManhattenDistance = Math.Abs(x - ClosestBeaconX) + Math.Abs(y - ClosestBeaconY);
    }

    /// <summary>
    /// Returns the number of positions that are covered by this sensor in the given Y row.
    /// </summary>
    /// <param name="referenceY">Y coordinate of the row to check.</param>
    /// <returns>Number of covered positions.</returns>
    public int GetBlockedXRangeForY(int referenceY)
    {
      return Math.Max(ManhattenDistance - Math.Abs(Y - referenceY), 0);
    }

    /// <summary>
    /// Returns the number of positions that are covered by this sensor in the given X column.
    /// </summary>
    /// <param name="referenceX">X coordinate of the column to check.</param>
    /// <returns>Number of covered positions.</returns>
    public int GetBlockedYRangeForX(int referenceX)
    {
      return Math.Max(ManhattenDistance - Math.Abs(X - referenceX), 0);
    }

    /// <summary>
    /// Returns an enumerable of all X coordinates that are within range of this sensor in the line with the given Y coordinate, based on this sensor's Manhatten distance.
    /// </summary>
    /// <param name="referenceY">Y coordinate of the line for which to find all covered X coordinates.</param>
    /// <returns>List of all X coordinates covered by this sensor in row Y.</returns>
    public List<int> GetCoveredXCoordinatesForY(int referenceY)
    {
      List<int> result = new();

      int remainingDistance = GetBlockedXRangeForY(referenceY);
      if (remainingDistance <= 0)
      {
        return result;
      }

      for (int x = X - remainingDistance; x < X + remainingDistance; x++)
      {
        result.Add(x);
      }
      return result;
    }

    /// <summary>
    /// Returns an enumerable of all Y coordinates that are within range of this sensor in the line with the given X coordinate, based on this sensor's Manhatten distance.
    /// </summary>
    /// <param name="referenceY">X coordinate of the line for which to find all covered Y coordinates.</param>
    /// <returns>List of all Y coordinates covered by this sensor in row X.</returns>
    public List<int> GetCoveredYCoordinatesForX(int referenceX)
    {
      List<int> result = new();

      int remainingDistance = GetBlockedYRangeForX(referenceX);
      if (remainingDistance <= 0)
      {
        return result;
      }

      for (int y = Y - remainingDistance; y < Y + remainingDistance; y++)
      {
        result.Add(y);
      }
      return result;
    }

    /// <summary>
    /// Gets all coordinates that are exactly outside of the Manhatten distance described by this sensor.
    /// </summary>
    /// <returns>List of all points on the edge.</returns>
    public IEnumerable<Point> GetAllPerimeterPoints()
    {
      for (int pointY = Y - ManhattenDistance - 1; pointY <= Y + ManhattenDistance + 1; pointY++)
      {
        int remainingDistance = Math.Max(ManhattenDistance + 1 - Math.Abs(Y - pointY), 0);
        Point a = new(X - remainingDistance, pointY);
        Point b = new(X + remainingDistance, pointY);
        yield return a;
        if (a != b)
        {
          yield return b;
        }
      }
    }

    public bool PointInRange(int checkX, int checkY)
    {
      int pointDistance = Math.Abs(X - checkX) + Math.Abs(Y - checkY);
      return pointDistance <= ManhattenDistance;
    }
  }
}