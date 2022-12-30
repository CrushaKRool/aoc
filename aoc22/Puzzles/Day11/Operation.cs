namespace aoc22.Puzzles.Day11
{
  internal class Operation
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