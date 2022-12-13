using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day04
{
  internal class Day04 : IPuzzleSolver
  {
    private readonly Regex rangePairParser = new("(\\d+)-(\\d+),(\\d+)-(\\d+)");

    public string PuzzleName => "Day 4: Camp Cleanup";

    public string SolvePart1(string input)
    {
      int numFullyContainedAssignments = 0;
      int numTotalAssignments = 0;
      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        Match match = rangePairParser.Match(line);
        if (match.Success)
        {
          int range1Start = int.Parse(match.Groups[1].Value);
          int range1End = int.Parse(match.Groups[2].Value);
          int range2Start = int.Parse(match.Groups[3].Value);
          int range2End = int.Parse(match.Groups[4].Value);

          Range range1 = new(range1Start, range1End);
          Range range2 = new(range2Start, range2End);
          if (range1.ContainsOther(range2) || range2.ContainsOther(range1))
          {
            numFullyContainedAssignments++;
          }
        }
        numTotalAssignments++;
      }
      return $"In {numFullyContainedAssignments} out of {numTotalAssignments} assignment pairs does one range fully contain the other.";
    }

    public string SolvePart2(string input)
    {
      int numOverlappingAssignments = 0;
      int numTotalAssignments = 0;
      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        Match match = rangePairParser.Match(line);
        if (match.Success)
        {
          int range1Start = int.Parse(match.Groups[1].Value);
          int range1End = int.Parse(match.Groups[2].Value);
          int range2Start = int.Parse(match.Groups[3].Value);
          int range2End = int.Parse(match.Groups[4].Value);

          Range range1 = new(range1Start, range1End);
          Range range2 = new(range2Start, range2End);
          if (range1.OverlapsOther(range2) || range2.OverlapsOther(range1))
          {
            numOverlappingAssignments++;
          }
        }
        numTotalAssignments++;
      }
      return $"In {numOverlappingAssignments} out of {numTotalAssignments} assignment pairs does one range overlap the other.";
    }

    /// <summary>
    /// Represents a range between a given minimum and maximum value.
    /// </summary>
    private sealed class Range
    {
      public int Min { get; }
      public int Max { get; }

      public Range(int min, int max)
      {
        if (min <= max)
        {
          Min = min;
          Max = max;
        }
        else
        {
          Min = max;
          Max = min;
        }
      }

      /// <summary>
      /// Indicates whether this range completely contains the given other range.
      /// </summary>
      /// <param name="other">Other range to compare this one to.</param>
      /// <returns>true, if this range fully contains the other.</returns>
      public bool ContainsOther(Range other)
      {
        return Min <= other.Min && Max >= other.Max;
      }

      /// <summary>
      /// Indicates whether this range overlaps the given other range.
      /// </summary>
      /// <param name="other">Other range to compare this one to.</param>
      /// <returns>true, if this range overlaps the other.</returns>
      public bool OverlapsOther(Range other)
      {
        return Min <= other.Min && Max >= other.Min || Min <= other.Max && Max >= other.Max;
      }
    }
  }
}