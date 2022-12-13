using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles
{
  class Day5 : IPuzzleSolver
  {
    private readonly Regex CommandParser = new("move (\\d+) from (\\d+) to (\\d+)");

    public string PuzzleName => "Day 5: Supply Stacks";

    public string SolvePart1(string input)
    {
      List<string> initialStackLines = new();
      IList<Stack<char>> stacks = null;

      foreach (string line in input.Split(Environment.NewLine))
      {
        if (stacks == null)
        {
          if (string.IsNullOrWhiteSpace(line))
          {
            stacks = ParseInitialStacks(initialStackLines);
          }
          else
          {
            initialStackLines.Add(line);
          }
        }
        else
        {
          ApplyMoveCommand9000(line, stacks);
        }
      }

      if (stacks == null)
      {
        throw new ArgumentException("Invalid input format. Initialization not clearly distinguished from commands by an empty line!");
      }

      StringBuilder sb = new();
      foreach (Stack<char> stack in stacks)
      {
        sb.Append(stack.Peek());
      }
      return $"The top crates on all stacks in order are: {sb}";
    }

    public string SolvePart2(string input)
    {
      List<string> initialStackLines = new();
      IList<Stack<char>> stacks = null;

      foreach (string line in input.Split(Environment.NewLine))
      {
        if (stacks == null)
        {
          if (string.IsNullOrWhiteSpace(line))
          {
            stacks = ParseInitialStacks(initialStackLines);
          }
          else
          {
            initialStackLines.Add(line);
          }
        }
        else
        {
          ApplyMoveCommand9001(line, stacks);
        }
      }

      if (stacks == null)
      {
        throw new ArgumentException("Invalid input format. Initialization not clearly distinguished from commands by an empty line!");
      }

      StringBuilder sb = new();
      foreach (Stack<char> stack in stacks)
      {
        sb.Append(stack.Peek());
      }
      return $"The top crates on all stacks in order are: {sb}";
    }

    private static IList<Stack<char>> ParseInitialStacks(List<string> stackLines)
    {
      IList<Stack<char>> stacks = new List<Stack<char>>();
      string indexLine = stackLines.Last();
      // For each stack index in the last line...
      for(int x = 0; x < indexLine.Length; x++)
      {
        if (char.IsWhiteSpace(indexLine[x]))
        {
          continue;
        }
        // ... go exactly at that x coordinate through all chars directly above in the previous lines.
        Stack<char> newStack = new();
        for (int y = stackLines.Count - 2; y >= 0; y--)
        {
          // If the crate here is not empty, add it to our new stack.
          char crate = stackLines[y][x];
          if (char.IsWhiteSpace(crate))
          {
            break;
          }
          newStack.Push(crate);
        }
        stacks.Add(newStack);
      }
      return stacks;
    }

    private void ApplyMoveCommand9000(string command, IList<Stack<char>> stacks)
    {
      Match match = CommandParser.Match(command);
      if (match.Success)
      {
        int numCratesToMove = int.Parse(match.Groups[1].Value);
        // Stack indices are 0-based internally and 1-based in the input.
        int stackSourceIndex = int.Parse(match.Groups[2].Value) - 1; 
        int stackTargetIndex = int.Parse(match.Groups[3].Value) - 1;
        for (int i = 0; i < numCratesToMove; i++)
        {
          char moved = stacks[stackSourceIndex].Pop();
          stacks[stackTargetIndex].Push(moved);
        }
      }
    }

    private void ApplyMoveCommand9001(string command, IList<Stack<char>> stacks)
    {
      Match match = CommandParser.Match(command);
      if (match.Success)
      {
        int numCratesToMove = int.Parse(match.Groups[1].Value);
        // Stack indices are 0-based internally and 1-based in the input.
        int stackSourceIndex = int.Parse(match.Groups[2].Value) - 1;
        int stackTargetIndex = int.Parse(match.Groups[3].Value) - 1;

        // Simple modification to the above logic:
        // By moving the crates to an intermediate stack first, we reverse the order once.
        // If we then move them from that intermediate stack to the real target,
        // they get reversed once more and are back in their original order.
        Stack<char> intermediateStack = new();
        for (int i = 0; i < numCratesToMove; i++)
        {
          char moved = stacks[stackSourceIndex].Pop();
          intermediateStack.Push(moved);
        }
        for (int i = 0; i < numCratesToMove; i++)
        {
          char moved = intermediateStack.Pop();
          stacks[stackTargetIndex].Push(moved);
        }
      }
    }
  }
}
