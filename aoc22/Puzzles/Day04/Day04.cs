using aoc_common;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day04
{
  internal class Day04 : IPuzzle
  {
    private readonly Regex rangePairParser = new("(\\d+)-(\\d+),(\\d+)-(\\d+)");

    public string PuzzleName => "Day 4: Camp Cleanup";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      int numFullyContainedAssignments = 0;
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
          if (range1.ContainsOther(range2) || range2.ContainsOther(range1))
          {
            numFullyContainedAssignments++;
          }
          if (range1.OverlapsOther(range2) || range2.OverlapsOther(range1))
          {
            numOverlappingAssignments++;
          }
        }
        numTotalAssignments++;
      }
      Console.WriteLine($"In {numFullyContainedAssignments} out of {numTotalAssignments} assignment pairs does one range fully contain the other.");
      Console.WriteLine($"In {numOverlappingAssignments} out of {numTotalAssignments} assignment pairs does one range overlap the other.");
    }
  }
}