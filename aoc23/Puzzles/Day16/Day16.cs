using aoc_common;

namespace aoc23.Puzzles.Day16
{
  public class Day16 : IPuzzle
  {
    public string PuzzleName => "Day 16: The Floor Will Be Lava";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Console.WriteLine();
      Console.WriteLine();
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      MirrorGrid grid = new(input);
      int energy = grid.GetEnergizedCountForStartConfiguration(-1, 0, EDirection.RIGHT);

      grid.PrintEnergized();

      Console.WriteLine($"Energized count: {energy}");
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      int highestEnergy = 0;
      MirrorGrid grid = new(input);
      Console.WriteLine("Left to Right");
      for (int y = 0; y < grid.YMax; y++)
      {
        int energy = grid.GetEnergizedCountForStartConfiguration(-1, y, EDirection.RIGHT);
        Console.WriteLine(energy);
        highestEnergy = Math.Max(energy, highestEnergy);
      }

      Console.WriteLine("Right to Left");
      for (int y = 0; y < grid.YMax; y++)
      {
        int energy = grid.GetEnergizedCountForStartConfiguration(grid.XMax, y, EDirection.LEFT);
        Console.WriteLine(energy);
        highestEnergy = Math.Max(energy, highestEnergy);
      }

      Console.WriteLine("Up to Down");
      for (int x = 0; x < grid.XMax; x++)
      {
        int energy = grid.GetEnergizedCountForStartConfiguration(x, -1, EDirection.DOWN);
        Console.WriteLine(energy);
        highestEnergy = Math.Max(energy, highestEnergy);
      }

      Console.WriteLine("Down to Up");
      for (int x = 0; x < grid.XMax; x++)
      {
        int energy = grid.GetEnergizedCountForStartConfiguration(x, grid.YMax, EDirection.UP);
        Console.WriteLine(energy);
        highestEnergy = Math.Max(energy, highestEnergy);
      }

      Console.WriteLine($"Highest energy: {highestEnergy}");
    }
  }
}
