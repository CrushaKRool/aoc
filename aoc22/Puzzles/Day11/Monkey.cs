using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day11
{
  internal class Monkey
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
      item.WorryLevel = (long)Math.Floor(item.WorryLevel / (double)WorryOperand);
    }
  }

  internal class MonkeyPart2 : Monkey
  {
    public MonkeyPart2(List<string> inputLines) : base(inputLines)
    {
    }

    protected override void ReduceWorry(Item item)
    {
      item.WorryLevel %= WorryOperand;
    }
  }
}