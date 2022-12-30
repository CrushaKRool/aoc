using aoc_common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day15
{
  public class Day15 : IPuzzle
  {
    private const int CoordinateMin = 0;
    private const int CoordinateMax = 4000000;

    private readonly Regex LineParser = new("Sensor at x=(-?\\d+), y=(-?\\d+): closest beacon is at x=(-?\\d+), y=(-?\\d+)");

    public string PuzzleName => "Day 15: Beacon Exclusion Zone";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<Sensor> sensors = new();
      foreach (string inputLine in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)))
      {
        Match m = LineParser.Match(inputLine);
        if (!m.Success)
        {
          throw new ArgumentException($"Unable to parse line {inputLine}.");
        }
        sensors.Add(new(
          int.Parse(m.Groups[1].Value),
          int.Parse(m.Groups[2].Value),
          int.Parse(m.Groups[3].Value),
          int.Parse(m.Groups[4].Value)
          ));
      }

      const int referenceY = 2000000;
      ISet<int> coveredPositions = new HashSet<int>();
      foreach (Sensor sensor in sensors)
      {
        foreach (int coveredX in sensor.GetCoveredXCoordinatesForY(referenceY))
        {
          coveredPositions.Add(coveredX);
        }
      }

      Console.WriteLine($"{coveredPositions.Count} can not contain a beacon.");

      // Part 2

      // To cut down on the number of coordinates we have to check, calculate the perimeter of each sensor.
      // Any points inside a sensor's range are no candidates, so only consider the points exactly outside of the edge bounds.
      // For each of them, check if they are within the range of other sensors.
      // Some perimeter points from different sensors may be overlapping.
      Point point = sensors
        .AsParallel()
        .SelectMany(sensor => sensor.GetAllPerimeterPoints())
        .Where(ep => PointWithinBounds(ep) && !PointInAnySensorRange(ep, sensors))
        .Distinct()
        .Single();

      BigInteger frequency = new BigInteger(point.X) * new BigInteger(4000000) + new BigInteger(point.Y);
      Console.WriteLine($"Found candidate at ({point.X},{point.Y}) with a frequency of {frequency}.");
    }

    private static bool PointInAnySensorRange(Point point, List<Sensor> sensors)
    {
      return sensors.Any(s => s.PointInRange(point.X, point.Y));
    }

    private static bool PointWithinBounds(Point point)
    {
      return point.X >= CoordinateMin && point.X <= CoordinateMax && point.Y >= CoordinateMin && point.Y <= CoordinateMax;
    }

    /// <summary>
    /// Visualization to debug the test input. Won't work with the actual input file due to its size.
    /// </summary>
    /// <param name="sensors"></param>
    private static void VisualizeEdgePoints(List<Sensor> sensors)
    {
      char[,] visualization = new char[50, 50];
      for (int x = 0; x < 50; x++)
      {
        for (int y = 0; y < 50; y++)
        {
          visualization[x, y] = '.';
        }
      }

      foreach (Sensor sensor in sensors)
      {
        foreach (Point ep in sensor.GetAllPerimeterPoints())
        {
          if (PointWithinBounds(ep))
          {
            visualization[ep.X, ep.Y] = 'o';
          }
        }
      }
      foreach (Sensor sensor in sensors)
      {
        if (PointWithinBounds(new Point(sensor.X, sensor.Y)))
        {
          visualization[sensor.X, sensor.Y] = 'S';
        }
        if (PointWithinBounds(new Point(sensor.ClosestBeaconX, sensor.ClosestBeaconY)))
        {
          visualization[sensor.ClosestBeaconX, sensor.ClosestBeaconY] = 'B';
        }
      }

      for (int y = 0; y < 50; y++)
      {
        for (int x = 0; x < 50; x++)
        {
          Console.Write(visualization[x, y]);
        }
        Console.WriteLine();
      }
    }
  }
}