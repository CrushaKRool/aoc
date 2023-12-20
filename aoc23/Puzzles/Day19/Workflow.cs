using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day19
{
  internal partial class Workflow
  {
    private static readonly Regex Parser = ParserRegex();

    public string Name { get; }
    public List<Rule> Rules { get; }

    public Workflow(string inputLine)
    {
      Match m = Parser.Match(inputLine);
      if (m.Success)
      {
        Name = m.Groups[1].Value;
        string[] ruleDefs = m.Groups[2].Value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Rules = ruleDefs.Select(r => new Rule(r)).ToList();
      }
      else
      {
        throw new ArgumentException("Unexpected input: " + inputLine);
      }
    }

    public string Evaluate(PartData data)
    {
      return Rules.Where(r => r.Matches(data)).Select(r => r.Result).First();
    }

    [GeneratedRegex(@"([a-z]+){(.*)}")]
    private static partial Regex ParserRegex();
  }
}
