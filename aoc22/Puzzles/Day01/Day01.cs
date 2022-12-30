using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day01
{
  public class Day01 : IPuzzle
  {
    public string PuzzleName => "Day 1: Calorie Counting";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<long> calorieSums = GetCalorieSums(input);

      // The puzzle actually only asks for the calorie amount, but let's output the index of the elf as well.
      long maxCalories = calorieSums.Max();
      long elfIndex = calorieSums.IndexOf(maxCalories);
      Console.WriteLine($"The Elf carrying the most is the {elfIndex + 1}th Elf with {maxCalories} Calories.");

      // By sorting, we lose track of the original index numbers in this data structure. But since those are not required in the answer...
      calorieSums.Sort();
      calorieSums.Reverse();
      long topThreeTotal = calorieSums.Take(3).Sum();
      Console.WriteLine($"The top 3 Elves are carrying a total of {topThreeTotal} Calories.");
    }

    private static List<long> GetCalorieSums(string input)
    {
      List<long> calorieSums = new();
      long curCalories = 0;

      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.TrimEntries))
      {
          if (string.IsNullOrEmpty(line))
          {
              calorieSums.Add(curCalories);
              curCalories = 0;
          }
          else
          {
              if (long.TryParse(line, out long parsedCalories))
              {
                  curCalories += parsedCalories;
              }
              else
              {
                  throw new ArgumentException("Invalid line in input:" + Environment.NewLine + line);
              }
          }
      }

      return calorieSums;
    }
  }
}
