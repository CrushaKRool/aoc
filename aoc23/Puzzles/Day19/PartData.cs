using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day19
{
  internal partial class PartData
  {
    private static readonly Regex Parser = ParserRegex();

    public Dictionary<string, int> Rating { get; } = [];

    public PartData(string inputLine)
    {
      Match match = Parser.Match(inputLine);
      if (!match.Success)
      {
        throw new ArgumentException("Unsupported part data: " + inputLine);
      }
      Rating["x"] = int.Parse(match.Groups[1].Value);
      Rating["m"] = int.Parse(match.Groups[2].Value);
      Rating["a"] = int.Parse(match.Groups[3].Value);
      Rating["s"] = int.Parse(match.Groups[4].Value);
    }

    [GeneratedRegex(@"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}")]
    private static partial Regex ParserRegex();
  }
}
