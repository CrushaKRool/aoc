using aoc_common;

namespace aoc23.Puzzles.Day20
{
  public class Day20 : IPuzzle
  {
    public string PuzzleName => "Day 20: Pulse Propagation";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      CPU cpu = new(input);
      for (int i = 0; i < 1000; i++)
      {
        cpu.PushButton();
        Console.WriteLine(cpu.HighCount + " * " + cpu.LowCount);
      }
      Console.WriteLine(cpu.HighCount * cpu.LowCount);
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      CPU cpu = new(input);
      long buttonPresses = 0;
      while (!cpu.LowRxSent)
      {
        buttonPresses++;
        cpu.PushButton();
      }
      Console.WriteLine($"Sent low pulse to rx module after {buttonPresses} button presses.");
    }
  }
}
