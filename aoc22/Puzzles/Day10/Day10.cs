using aoc_common;
using System;
using System.Collections.Generic;
using System.Text;

namespace aoc22.Puzzles.Day10
{
  internal class Day10 : IPuzzle
  {
    public string PuzzleName => "Day 10: Cathode-Ray Tube";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Queue<string> instructionQueue = new(input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));
      SimpleCpu cpu = new(instructionQueue);
      int signalStrengthSum = 0;
      while (!cpu.FinishedAllInstructions)
      {
        cpu.Tick();
        if ((cpu.Cycle - 20) % 40 == 0)
        {
          signalStrengthSum += cpu.Cycle * cpu.X;
        }
      }

      Console.WriteLine($"The sum of signal strengths is {signalStrengthSum}.");

      // Part 2

      StringBuilder sb = new();

      instructionQueue = new(input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));
      cpu = new(instructionQueue);
      while (!cpu.FinishedAllInstructions)
      {
        if (Math.Abs(cpu.X - (cpu.Cycle - 1) % 40) < 2)
        {
          sb.Append('#');
        }
        else
        {
          sb.Append('.');
        }
        if (cpu.Cycle % 40 == 0)
        {
          sb.AppendLine();
        }
        cpu.Tick();
      }

      Console.WriteLine(sb.ToString());
    }
  }
}