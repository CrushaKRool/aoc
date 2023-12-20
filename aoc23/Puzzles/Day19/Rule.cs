using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day19
{
  internal partial class Rule
  {
    private static readonly Regex Parser = RuleParser();

    private string? Category { get; }
    private string? Operator { get; }
    private int? CheckValue { get; }

    public string Result { get; }

    public Rule(string ruleLine)
    {
      Match m = Parser.Match(ruleLine);
      if (m.Success)
      {
        Category = m.Groups[1].Value;
        Operator = m.Groups[2].Value;
        CheckValue = int.Parse(m.Groups[3].Value);
        Result = m.Groups[4].Value;
      }
      else
      {
        Result = ruleLine;
      }
    }

    public bool Matches(PartData data)
    {
      if (string.IsNullOrEmpty(Operator))
      {
        return true;
      }
      int dataValue = data.Rating[Category];
      return Operator switch
      {
        "<" => dataValue < CheckValue,
        ">" => dataValue > CheckValue,
        _ => throw new ArgumentException("Unsupported operator: " + Operator),
      };
    }

    [GeneratedRegex(@"([xmas])([<>])(\d+):([a-z]+|A|R)")]
    private static partial Regex RuleParser();
  }
}
