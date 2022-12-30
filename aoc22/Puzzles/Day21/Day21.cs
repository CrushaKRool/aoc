using aoc_common;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day21
{
  public class Day21 : IPuzzle
  {
    public string PuzzleName => "Day 21: Monkey Math";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      IDictionary<string, Monkey> allMonkeys = new Dictionary<string, Monkey>();
      foreach (string line in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        string[] split = line.Split(": ");
        Monkey m = new(split[0], split[1]);
        allMonkeys.Add(m.Name, m);
      }

      Monkey root = allMonkeys["root"];
      long yelled = root.GetYelledNumber(allMonkeys);
      Console.WriteLine($"The root monkey will yell the number {yelled}.");

      // Part 2

      // Change job of root and reset cache, so we don't get the same answer as before.
      root.JobExpression = Regex.Replace(root.JobExpression, "([+\\-*/])", "=");

      Monkey human = allMonkeys["humn"];
      long humanNumber = 0;
      long humanIncrement = 1;
      long lastYelled;

      // Simple brute-force approach:
      // We try yelling a number and see whether that gets our result closer to zero.
      // If we get closer, we keep increasing the incrementing in this direction to get there faster.
      // If we get further away, change the strategy and try to go small steps into the opposite direction.
      while (true)
      {
        if (humanIncrement % 1000 == 0)
        {
          Console.WriteLine($"Root monkey yelled {yelled}; Human number: {humanNumber}; Human increment: {humanIncrement}.");
        }
        human.JobExpression = humanNumber.ToString();

        // We don't know which monkeys depend on the human in a chain (may be more than one chain),
        // so we must reset all of them to get fresh results from the new number the human is now yelling.
        foreach (Monkey m in allMonkeys.Values)
        {
          m.ResetCache();
        }

        lastYelled = yelled;
        yelled = root.GetYelledNumber(allMonkeys);
        if (yelled == 0)
        {
          Console.WriteLine($"The human must yell {humanNumber}.");
          break;
        }
        if (Math.Abs(yelled) - Math.Abs(lastYelled) > 0)
        {
          // Error value is increasing, so change direction and start in small steps.
          humanIncrement = -1 * Math.Sign(humanIncrement);
        }
        else
        {
          // Error value is decreasing, so keep going faster.
          humanIncrement += 1 * Math.Sign(humanIncrement);
        }
        // Increment by the square of our current increment, to get even faster to the result.
        humanNumber += humanIncrement * humanIncrement * Math.Sign(humanIncrement);
      }
    }
  }
}