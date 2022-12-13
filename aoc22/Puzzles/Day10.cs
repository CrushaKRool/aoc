using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aoc22.Puzzles
{
  class Day10 : IPuzzleSolver
  {
    public string PuzzleName => "Day 10: Cathode-Ray Tube";

    public string SolvePart1(string input)
    {
      Queue<string> instructionQueue = new(input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)));
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

      return $"The sum of signal strengths is {signalStrengthSum}.";
    }

    public string SolvePart2(string input)
    {
      StringBuilder sb = new();

      Queue<string> instructionQueue = new(input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)));
      SimpleCpu cpu = new(instructionQueue);
      while (!cpu.FinishedAllInstructions)
      {
        if (Math.Abs(cpu.X - ((cpu.Cycle-1) % 40)) < 2)
        {
          sb.Append('#');
        }
        else
        {
          sb.Append('.');
        }
        if ((cpu.Cycle) % 40 == 0)
        {
          sb.AppendLine();
        }
        cpu.Tick();
      }

      return sb.ToString();
    }

    class SimpleCpu
    {
      public int Cycle { get; set; } = 1;
      public int X { get; set; } = 1;

      public bool FinishedAllInstructions { get => InstructionQueue.Count == 0; }

      private readonly Queue<string> InstructionQueue;

      private string? CurInstruction;
      private int CurInstructionCycleStart;

      public SimpleCpu(Queue<string> instructionQueue)
      {
        InstructionQueue = instructionQueue;
      }

      public void Tick()
      {
        if (string.IsNullOrEmpty(CurInstruction))
        {
          if (FinishedAllInstructions)
          {
            return;
          }
          CurInstruction = InstructionQueue.Dequeue();
          CurInstructionCycleStart = Cycle;
        }
        bool finishedProcessing = ProcessInstruction(CurInstruction);
        if (finishedProcessing)
        {
          CurInstruction = null;
        }
        Cycle++;
      }

      private bool ProcessInstruction(string instruction)
      {
        string[] instructionParts = instruction.Split(" ");
        switch (instructionParts[0])
        {
          case "noop":
            return true;
          case "addx":
            if (Cycle - CurInstructionCycleStart >= 1)
            {
              X += int.Parse(instructionParts[1]);
              return true;
            }
            return false;
          default:
            throw new ArgumentException($"Unknown instruction: {instruction}");
        }
      }
    }
  }
}
