#undef TRACE
#undef DEBUG

using aoc_common;
using System;

namespace aoc22.Puzzles.Day17
{
  public class Day17 : IPuzzle
  {
    public string PuzzleName => "Day 17: Pyroclastic Flow";

    public string InputFileName => @"Input.txt";

    /// <summary>
    /// Shape definitions of all the rock parts, in the order they fall.
    /// Offsets are relative to the bottom left corner.
    /// </summary>
    private readonly RockDefinition[] RockDefinitions = new RockDefinition[]
    {
      new RockDefinition(new(0, 0), new(1, 0), new(2, 0), new(3, 0)),
      new RockDefinition(new(0, 1), new(1, 0), new(1, 1), new(1, 2), new(2, 1)),
      new RockDefinition(new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2)),
      new RockDefinition(new(0, 0), new(0, 1), new(0, 2), new(0, 3)),
      new RockDefinition(new(0, 0), new(0, 1), new(1, 0), new(1, 1))
    };

    public void Run(string input)
    {
      // Sanitize input, so we don't need to deal with trailing whitespace.
      input = input.Trim();

      RunSimulation(input, 2022);
      RunSimulation(input, 1000000000000);
    }

    private void RunSimulation(string input, long maxRocks)
    {
      Grid grid = new();

      int jetstreamIndex = 0;
      int curRockIndex = 0;
      long numRocksPlaced = 0;

      while (true)
      {
        // Need to have enough free lines in the data structure to accomodate the start offset of the rock as well as the maximum height of a rock shape.
        grid.EnsureSpace(8);

        // Spawn a new rock.
        Rock curRock = RockDefinitions[curRockIndex++ % RockDefinitions.Length]
          .SpawnRock(2, grid.HighestRockRow + 5);
        // +5 because our row is 0-indexed and -1 is the baseline. And we want an offset of 3 free spaces to the rock start.

        while (curRock.CanFall(grid))
        {
          curRock.Y--;
#if TRACE
          Console.WriteLine("Making rock fall");
          grid.PrintGridSection(grid.HighestRockRow - 10, grid.HighestRockRow + 3, curRock);
#endif
          char jetstream = input[jetstreamIndex++ % input.Length];
          switch (jetstream)
          {
            case '<':
              if (curRock.CanMoveSideways(grid, -1))
              {
                curRock.X--;
              }
#if TRACE
              Console.WriteLine("Making rock move <");
#endif
              break;
            case '>':
              if (curRock.CanMoveSideways(grid, 1))
              {
                curRock.X++;
              }
#if TRACE
              Console.WriteLine("Making rock move >");
#endif
              break;
          }
#if TRACE
          grid.PrintGridSection(grid.HighestRockRow - 10, grid.HighestRockRow + 3, curRock);
#endif
        }
        grid.FixateRock(curRock);
        numRocksPlaced++;
        if (numRocksPlaced % 10000 == 0)
        {
          Console.WriteLine($"Placed {numRocksPlaced} rocks.");
        }
#if DEBUG
        grid.PrintGridSection(grid.HighestRockRow - 10, grid.HighestRockRow + 3, null);
#endif
        if (numRocksPlaced >= maxRocks)
        {
          // +1 because our rows are 0-indexed.
          Console.WriteLine($"The highest rock line after {maxRocks} rocks is at {grid.HighestRockRow + 1}.");
          break;
        }
      }
    }
  }
}