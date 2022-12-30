#undef TRACE

using aoc_common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace aoc22.Puzzles.Day23
{
  public class Day23 : IPuzzle
  {
    public string PuzzleName => "Day 23: Unstable Diffusion";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<Elf> allElves = ReadElvesFromInput(input);

      PrintAllElves(allElves);

      for (int i = 0; i < 10; i++)
      {
        ProcessRound(allElves, i);
      }

      Rectangle mbr = GetMinimumBoundingRectangle(allElves);
      Console.WriteLine($"The minimum bounding rectangle after 10 rounds will contain {mbr.Width * mbr.Height - allElves.Count} empty ground tiles.");

      // Part 2

      allElves = ReadElvesFromInput(input);

      int round = 0;
      bool elfMoved;
      do
      {
        elfMoved = ProcessRound(allElves, round);
        round++;
      } while (elfMoved); // Run until no elves moved.

      PrintAllElves(allElves);

      Console.WriteLine($"{round} is the first round where no elves moved.");
    }

    private static List<Elf> ReadElvesFromInput(string input)
    {
      List<Elf> allElves = new();
      int y = 0;
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        for (int x = 0; x < inputLine.Length; x++)
        {
          if (inputLine[x] == '#')
          {
            allElves.Add(new Elf { Location = new Point(x, y) });
          }
        }
        y++;
      }
      return allElves;
    }

    /// <summary>
    /// Advances the elves a single round by letting each propose a new destination and then moving those that won't overlap at the new destination.
    /// </summary>
    /// <param name="allElves">Enumeration of all elves.</param>
    /// <param name="round">The current round. This value is used to decide which direction to consider first.</param>
    /// <returns>true, if at least one elf moved this round. false, is everyone is in an unmovable position.</returns>
    private static bool ProcessRound(IEnumerable<Elf> allElves, int round)
    {
#if DEBUG
      Console.WriteLine($"Starting round {round + 1}.");
#else
      if (round % 100 == 0)
      {
        Console.WriteLine($"Starting round {round + 1}.");
      }
#endif

      // Have all elves make a proposal.
      foreach (Elf elf in allElves)
      {
        elf.ProposeLocation(allElves, round);
      }

#if TRACE
      PrintAllElves(allElves);
#else
      if (round % 100 == 0)
      {
        PrintAllElves(allElves);
      }
#endif

      // Find all proposals that are not overlapping.
      ISet<Point> distinctProposals = allElves
        .Select(elf => elf.ProposedLocation)
        .GroupBy(p => p)
        .Where(grp => grp.Count() == 1)
        .Select(grp => grp.Key)
        .ToHashSet();

      bool elfMoved = false;

      // Move all elves to the proposed location that don't have an overlap.
      foreach (Elf elf in allElves.Where(elf => elf.ProposedMove != null && distinctProposals.Contains(elf.ProposedLocation)))
      {
        elf.Location = elf.ProposedLocation;
        elf.ProposedMove = null;
        elfMoved = true;
      }

      return elfMoved;
    }

    private static Rectangle GetMinimumBoundingRectangle(IEnumerable<Elf> allElves)
    {
      int xMin = int.MaxValue;
      int xMax = int.MinValue;
      int yMin = int.MaxValue;
      int yMax = int.MinValue;

      foreach (Point location in allElves.Select(elf => elf.Location))
      {
        xMin = Math.Min(location.X, xMin);
        xMax = Math.Max(location.X, xMax);
        yMin = Math.Min(location.Y, yMin);
        yMax = Math.Max(location.Y, yMax);
      }

      return Rectangle.FromLTRB(xMin, yMin, xMax + 1, yMax + 1);
    }

    private static void PrintAllElves(IEnumerable<Elf> allElves)
    {
      IDictionary<Point, Elf> allElfLocations = allElves.ToDictionary(elf => elf.Location, elf => elf);
      Rectangle rect = GetMinimumBoundingRectangle(allElves);
      for (int y = 0; y < rect.Height; y++)
      {
        for (int x = 0; x < rect.Width; x++)
        {
          Point curPoint = new(x + rect.X, y + rect.Y);
          if (allElfLocations.ContainsKey(curPoint))
          {
            string? proposedMoveSymbol = allElfLocations[curPoint].ProposedMove?.Symbol;
            if (proposedMoveSymbol == null)
            {
              Console.Write("#");
            }
            else
            {
              Console.ForegroundColor = proposedMoveSymbol switch
              {
                "N" => ConsoleColor.Red,
                "E" => ConsoleColor.Magenta,
                "S" => ConsoleColor.Green,
                "W" => ConsoleColor.Blue,
                _ => ConsoleColor.White,
              };
              Console.Write(proposedMoveSymbol);
              Console.ResetColor();
            }
          }
          else
          {
            Console.Write('.');
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}