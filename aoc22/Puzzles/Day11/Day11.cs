using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day11
{
  internal class Day11 : IPuzzle
  {
    private const int LinesPerMonkey = 7;

    public string PuzzleName => "Day 11: Monkey in the Middle";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      IDictionary<int, Monkey> monkeys = new SortedDictionary<int, Monkey>();
      List<string> inputLines = input.Split(Environment.NewLine).ToList();
      for (int i = 0; i <= inputLines.Count - LinesPerMonkey; i += LinesPerMonkey)
      {
        List<string> subList = inputLines.GetRange(i, LinesPerMonkey);
        Monkey monkey = new(subList);
        monkeys.Add(monkey.ID, monkey);
      }

      for (int i = 0; i < 20; i++)
      {
        foreach (Monkey monkey in monkeys.Values)
        {
          monkey.HandleItems(monkeys);
        }
      }

      List<Monkey> monkeyRanking = monkeys.Values.OrderByDescending(m => m.ActivityLevel).ToList();
      long monkeyBusiness = monkeyRanking[0].ActivityLevel * monkeyRanking[1].ActivityLevel;

      Console.WriteLine($"The level of monkey business after 20 rounds is {monkeyBusiness}.");

      // Part 2

      monkeys = new SortedDictionary<int, Monkey>();
      for (int i = 0; i <= inputLines.Count - LinesPerMonkey; i += LinesPerMonkey)
      {
        List<string> subList = inputLines.GetRange(i, LinesPerMonkey);
        Monkey monkey = new MonkeyPart2(subList);
        monkeys.Add(monkey.ID, monkey);
      }

      // To prevent the worry values from becoming unmanagably large, we modulo them by the multiple of all monkey's test divisors.
      // This works because we are not interested in the exact worry values but only whether they are divisible by the respective monkey's tests.
      // And since all divisors are prime, this does not influence the relative outcome of those tests.
      long monkeyDivisorMultiple = 1;
      foreach (Monkey monkey in monkeys.Values)
      {
        monkeyDivisorMultiple *= monkey.TestDivisibleOperand;
      }
      foreach (Monkey monkey in monkeys.Values)
      {
        monkey.WorryOperand = monkeyDivisorMultiple;
      }

      for (int i = 0; i < 10000; i++)
      {
        foreach (Monkey monkey in monkeys.Values)
        {
          monkey.HandleItems(monkeys);
        }
      }

      monkeyRanking = monkeys.Values.OrderByDescending(m => m.ActivityLevel).ToList();
      monkeyBusiness = monkeyRanking[0].ActivityLevel * monkeyRanking[1].ActivityLevel;

      Console.WriteLine($"The level of monkey business after 10000 rounds without reducing worry is {monkeyBusiness}.");
    }
  }
}