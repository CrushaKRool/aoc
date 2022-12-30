using System;
using System.Collections.Generic;

namespace aoc22.Puzzles.Day10
{
  internal class SimpleCpu
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