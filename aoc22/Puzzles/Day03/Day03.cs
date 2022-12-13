using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day03
{
  internal class Day03 : IPuzzle
  {
    public string PuzzleName => "Day 3: Rucksack Reorganization";

    public string InputFileName => @"Puzzles\Day03\Day03Input.txt";

    public void Run(string input)
    {
      int numRucksacks = 0;
      int prioritySum = 0;
      foreach (string line in input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)))
      {
        int splitIndex = line.Length / 2;
        string items1 = line[..splitIndex];
        string items2 = line[splitIndex..];

        ISet<char> compartment1 = items1.ToHashSet();
        try
        {
          char duplicateItem = items2.First(c => compartment1.Contains(c));
          prioritySum += CharToPriority(duplicateItem);
        }
        catch (Exception ex)
        {
          throw new ArgumentException($"Error while processing line {line}!", ex);
        }
        numRucksacks++;
      }

      Console.WriteLine($"The total sum of item priorities that appear in both compartments of the {numRucksacks} rucksacks is {prioritySum}.");

      // Part 2

      const int stepSize = 3;

      int numGroups = 0;
      prioritySum = 0;
      string[] lines = input.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
      for (int i = 0; i < lines.Length; i += stepSize)
      {
        ISet<char> commonChars = lines[i].ToHashSet();
        for (int j = 1; j < stepSize; j++)
        {
          commonChars.IntersectWith(lines[i + j]);
        }
        char badgeItem = commonChars.First();
        prioritySum += CharToPriority(badgeItem);
        numGroups++;
      }

      Console.WriteLine($"The total sum of group badge priorities of the {numGroups} groups is {prioritySum}.");
    }

    /// <summary>
    /// Returns the numeric priority of the respective char item.
    /// </summary>
    /// <param name="c">Character for which to get the priority.</param>
    /// <returns>a = 1, ... , z = 26, A = 27, ... , Z = 52</returns>
    private static int CharToPriority(char c)
    {
      if (char.IsLower(c))
      {
        return c - 'a' + 1;
      }
      if (char.IsUpper(c))
      {
        return c - 'A' + 27;
      }
      return 0;
    }
  }
}