using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles
{
  class Day11 : IPuzzleSolver
  {
    private const int LinesPerMonkey = 7;

    public string PuzzleName => "Day 11: Monkey in the Middle";

    public string SolvePart1(string input)
    {
      IDictionary<int, Monkey> monkeys = new SortedDictionary<int, Monkey>();
      List<string> inputLines = input.Split(Environment.NewLine).ToList();
      for (int i = 0; i <= inputLines.Count - LinesPerMonkey; i+= LinesPerMonkey)
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

      return $"The level of monkey business after 20 rounds is {monkeyBusiness}.";
    }

    public string SolvePart2(string input)
    {
      IDictionary<int, Monkey> monkeys = new SortedDictionary<int, Monkey>();
      List<string> inputLines = input.Split(Environment.NewLine).ToList();
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

      List<Monkey> monkeyRanking = monkeys.Values.OrderByDescending(m => m.ActivityLevel).ToList();
      long monkeyBusiness = monkeyRanking[0].ActivityLevel * monkeyRanking[1].ActivityLevel;

      return $"The level of monkey business after 10000 rounds without reducing worry is {monkeyBusiness}.";
    }

    class Monkey
    {
      public int ID { get; set; }
      public Queue<Item> Items { get; set; }
      public Operation Operation { get; set; }
      public long TestDivisibleOperand { get; set; }
      public int TargetMonkeyIfTestTrue { get; set; }
      public int TargetMonkeyIfTestFalse { get; set; }

      public long WorryOperand { get; set; } = 3;
      public long ActivityLevel { get; set; }

      public Monkey(List<string> inputLines)
      {
        int idx = 0;
        ID = int.Parse(Regex.Match(inputLines[idx++], "Monkey (\\d+):").Groups[1].Value);
        Items = new Queue<Item>(Regex.Match(inputLines[idx++], "Starting items: (.+)").Groups[1].Value
          .Split(", ")
          .Select(s => long.Parse(s.Trim()))
          .Select(l => new Item() { WorryLevel = l }));
        Match m = Regex.Match(inputLines[idx++], "Operation: new = old ([+\\-*/%]) (\\S+)");
        Operation = new Operation(m.Groups[1].Value, m.Groups[2].Value);
        TestDivisibleOperand = int.Parse(Regex.Match(inputLines[idx++], "Test: divisible by (\\d+)").Groups[1].Value);
        TargetMonkeyIfTestTrue = int.Parse(Regex.Match(inputLines[idx++], "If true: throw to monkey (\\d+)").Groups[1].Value);
        TargetMonkeyIfTestFalse = int.Parse(Regex.Match(inputLines[idx], "If false: throw to monkey (\\d+)").Groups[1].Value);
      }

      public void HandleItems(IDictionary<int, Monkey> allMonkeys)
      {
        while (Items.TryDequeue(out Item? item))
        {
          if (item == null)
          {
            continue;
          }

          ActivityLevel++;

          // Inspect
          Operation.ProcessItem(item);

          // Reduce worry
          ReduceWorry(item);

          // Test and throw
          if (item.WorryLevel % TestDivisibleOperand == 0)
          {
            allMonkeys[TargetMonkeyIfTestTrue].Items.Enqueue(item);
          }
          else
          {
            allMonkeys[TargetMonkeyIfTestFalse].Items.Enqueue(item);
          }
        }
      }

      protected virtual void ReduceWorry(Item item)
      {
        item.WorryLevel = (long)(Math.Floor(item.WorryLevel / (double)WorryOperand));
      }
    }

    class MonkeyPart2 : Monkey
    {
      public MonkeyPart2(List<string> inputLines) : base(inputLines)
      {
      }

      protected override void ReduceWorry(Item item)
      {
        item.WorryLevel %= WorryOperand;
      }
    }

    class Item
    {
      public long WorryLevel { get; set; }
    }

    class Operation
    {
      public string Operator { get; }
      public string Operand { get; }

      public Operation(string op, string operand)
      {
        Operator = op;
        Operand = operand;
      }

      public void ProcessItem(Item item)
      {
        long numOperand;
        if ("old".Equals(Operand))
        {
          numOperand = item.WorryLevel;
        }
        else
        {
          numOperand = long.Parse(Operand);
        }
        switch (Operator)
        {
          case "+":
            item.WorryLevel += numOperand;
            break;
          case "*":
            item.WorryLevel *= numOperand;
            break;
        }
      }
    }
  }
}
