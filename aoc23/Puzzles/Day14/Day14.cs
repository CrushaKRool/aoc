using aoc_common;

namespace aoc23.Puzzles.Day14
{
  public class Day14 : IPuzzle
  {
    public string PuzzleName => "Day 14: Parabolic Reflector Dish";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Console.WriteLine("------------------------------------------------");
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      Platform platform = new(input);
      platform.TiltNorth();
      platform.Print();
      long load = platform.MeasureNorthLoad();
      Console.WriteLine($"North load is {load}.");
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      Platform platform = new(input);
      for (int i = 0; i < 1000000000; i++)
      {
        switch (i % 4)
        {
          case 0:
            platform.TiltNorth();
            break;
          case 1:
            platform.TiltWest();
            break;
          case 2:
            platform.TiltSouth();
            break;
          case 3:
            platform.TiltEast();
            break;
        }
        if (i % 1000 == 0)
        {
          Console.WriteLine($"{i} of 1000000000 => {i / 1000000000.0 * 100}%");
        }
      }
      long load = platform.MeasureNorthLoad();
      Console.WriteLine($"North load is {load}.");
    }
  }
}
