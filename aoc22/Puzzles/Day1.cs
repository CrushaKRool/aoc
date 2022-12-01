using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace aoc22.Puzzles
{
  class Day1 : IPuzzleSolver
  {
    public string PuzzleName => "Day 1: Calorie Counting";

    public string Compute(string input)
    {
      return Part1(input) + Environment.NewLine + Part2(input);
    }

    /// <summary>
    /// How many calories is the elf with the most calories carrying?
    /// </summary>
    /// <param name="input">One numeric calorie per line, or a blank line to indicate the end of the current elf's load.</param>
    private static string Part1(string input)
    {
      List<long> calorieSums = GetCalorieSums(input);

      // The puzzle actually only asks for the calorie amount, but let's output the index of the elf as well.
      long maxCalories = calorieSums.Max();
      long elfIndex = calorieSums.IndexOf(maxCalories);
      return $"The Elf carrying the most is the {elfIndex + 1}th Elf with {maxCalories} Calories.";
    }

    /// <summary>
    /// How many total calories are the three elves with the most calories carrying?
    /// </summary>
    /// <param name="input">One numeric calorie per line, or a blank line to indicate the end of the current elf's load.</param>
    private static string Part2(string input)
    {
      List<long> calorieSums = GetCalorieSums(input);

      // By sorting, we lose track of the original index numbers in this data structure. But since those are not required in the answer...
      calorieSums.Sort();
      calorieSums.Reverse();
      long topThreeTotal = calorieSums.Take(3).Sum();
      return $"The top 3 Elves are carrying a total of {topThreeTotal} Calories.";
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
