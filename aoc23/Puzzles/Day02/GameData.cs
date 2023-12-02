using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day02
{
  internal partial class GameData
  {
    public string InputLine { get; }
    public int GameNumber { get; }
    public int MaxRed { get; }
    public int MaxGreen { get; }
    public int MaxBlue { get; }
    public int PowerOfMinimumSet { get; }

    private static readonly Regex Parser = GameParser();

    public GameData(string line)
    {
      InputLine = line;
      Match match = Parser.Match(line);
      if (!match.Success)
      {
        throw new ArgumentException("Unexpected input format: " + line, nameof(line));
      }
      GameNumber = int.Parse(match.Groups[1].Value);
      string[] cubeSets = match.Groups[2].Value.Split(';', StringSplitOptions.TrimEntries);
      foreach (string cs in cubeSets)
      {
        CubeSet cubeSet = ParseCubeSet(cs);
        MaxRed = Math.Max(MaxRed, cubeSet.Red);
        MaxGreen = Math.Max(MaxGreen, cubeSet.Green);
        MaxBlue = Math.Max(MaxBlue, cubeSet.Blue);
      }
      PowerOfMinimumSet = MaxRed * MaxGreen * MaxBlue;
    }

    private CubeSet ParseCubeSet(string part)
    {
      CubeSet result = new();
      string[] cubes = part.Split(',', StringSplitOptions.TrimEntries);
      foreach (string pair in cubes)
      {
        string[] countAndColor = pair.Split(' ', StringSplitOptions.TrimEntries);
        int count = int.Parse(countAndColor[0]);
        string color = countAndColor[1];
        switch (color)
        {
          case "red":
            result.Red = count;
            break;
          case "green":
            result.Green = count;
            break;
          case "blue":
            result.Blue = count;
            break;
        }
      }
      return result;
    }

    private struct CubeSet
    {
      public int Red { get; set; }
      public int Green { get; set; }
      public int Blue { get; set; }
    }

    [GeneratedRegex(@"^Game (\d+): (.+)$")]
    private static partial Regex GameParser();
  }
}
