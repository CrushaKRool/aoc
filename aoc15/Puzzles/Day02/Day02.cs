using aoc_common;

namespace aoc15.Puzzles.Day02
{
  public class Day02 : IPuzzle
  {
    public string InputFileName => @"Puzzles\Day02\Day02Input.txt";

    public string PuzzleName => "Day 2: I Was Told There Would Be No Math";

    public void Run(string input)
    {
      int totalSqFeet = 0;
      int totalRibbonFeet = 0;
      foreach(string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)))
      {
        int[] dimensions = line.Split("x").Select(s => int.Parse(s)).ToArray();
        List<int> areasPerSide = new()
        {
          dimensions[0] * dimensions[1],
          dimensions[0] * dimensions[1],
          dimensions[1] * dimensions[2],
          dimensions[1] * dimensions[2],
          dimensions[2] * dimensions[0],
          dimensions[2] * dimensions[0],
        };
        // Calculate sum of all sides plus the smallest side as slack.
        totalSqFeet += areasPerSide.Sum() + areasPerSide.Min();

        // Part 2: Sort the dimensions, so we can find the smallest ones.
        Array.Sort(dimensions);
        // Calculate smallest perimeter and cubic feet of volume.
        totalRibbonFeet += (2 * dimensions[0] + 2 * dimensions[1]) + (dimensions[0] * dimensions[1] * dimensions[2]);
      }
      Console.WriteLine($"A total of {totalSqFeet} square feet of paper is needed.");
      Console.WriteLine($"A total of {totalRibbonFeet} feet is needed for the ribbon.");
    }
  }
}
