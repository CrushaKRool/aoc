using aoc_common;
using System.Drawing;

namespace aoc23.Puzzles.Day03
{
  public class Day03 : IPuzzle
  {
    public string PuzzleName => "Day 3: Gear Ratios";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Grid grid = new(input);
      List<GridNumber> numbers = grid.FindGridNumbers();
      Part1(grid, numbers);
      Part2(grid, numbers);
    }

    private void Part1(Grid grid, List<GridNumber> numbers)
    {
      Console.WriteLine("Part 1:");
      int sum = 0;
      foreach (var gridNum in numbers)
      {
        bool isAdjacent = grid.IsNumberAdjacentToSymbol(gridNum);
        if (isAdjacent)
        {
          sum += gridNum.NumericValue;
        }
      }
      Console.WriteLine("Sum of part numbers: " + sum);
    }

    private void Part2(Grid grid, List<GridNumber> numbers)
    {
      Console.WriteLine("Part 2:");
      int sum = 0;
      List<Point> gears = grid.FindGears();
      foreach (Point gear in gears)
      {
        var touchingNums = numbers.Where(gn => gn.GetSurroundingPoints().Contains(gear)).ToList();
        if (touchingNums.Count == 2)
        {
          sum += touchingNums[0].NumericValue * touchingNums[1].NumericValue;
        }
      }
      Console.WriteLine("Sum of gear ratios: " + sum);
    }
  }
}
