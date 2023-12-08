using aoc_common;
using System.Text.RegularExpressions;
using System.Linq;

namespace aoc23.Puzzles.Day08
{
  public partial class Day08 : IPuzzle
  {
    public string PuzzleName => "Haunted Wasteland";

    public string InputFileName => @"Input.txt";

    private static readonly Regex NODE_PARSER = NodeParser();

    public void Run(string input)
    {
      string[] inputLines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      string instructions = inputLines[0];
      Dictionary<string, Node> nodes = [];
      for (int i = 1; i < inputLines.Length; i++)
      {
        Match m = NODE_PARSER.Match(inputLines[i]);
        if (m.Success)
        {
          string name = m.Groups[1].Value;
          string left = m.Groups[2].Value;
          string right = m.Groups[3].Value;
          nodes[name] = new Node(name, left, right);
        }
      }

      Part1(instructions, nodes);

      Console.WriteLine("-------------------------------------------------------------");

      Part2(instructions, nodes);
    }

    private static void Part1(string instructions, Dictionary<string, Node> nodes)
    {
      Console.WriteLine("Part 1:");

      Pathfinder pf = new("AAA");
      while (!"ZZZ".Equals(pf.NextNode))
      {
        pf.Move(instructions, nodes);
        Console.WriteLine(pf.NextNode);
      }

      Console.WriteLine($"Reached destination in {pf.Steps} steps.");
    }

    private static void Part2(string instructions, Dictionary<string, Node> nodes)
    {
      Console.WriteLine("Part 2:");

      List<Pathfinder> pathfinders = [];
      pathfinders.AddRange(nodes.Keys.Where(nodeName => nodeName.EndsWith('A')).Select(nodeName => new Pathfinder(nodeName)));

      foreach (var pf in pathfinders)
      {
        while (!pf.NextNode.EndsWith('Z'))
        {
          pf.Move(instructions, nodes);
        }
        Console.WriteLine($"{pf.StartNode} -> {pf.NextNode} in {pf.Steps}");
      }

      long totalSteps = pathfinders.Select(pf => pf.Steps).Aggregate(LCM);
      Console.WriteLine($"All reached destinations in {totalSteps} steps.");
    }

    private static long GCD(long a, long b)
    {
      long remainder;

      while (b != 0)
      {
        remainder = a % b;
        a = b;
        b = remainder;
      }

      return a;
    }

    private static long LCM(long a, long b)
    {
      return (a * b) / GCD(a, b);
    }

    [GeneratedRegex(@"([A-Z1-9]+) = \(([A-Z1-9]+), ([A-Z1-9]+)\)")]
    private static partial Regex NodeParser();
  }
}
