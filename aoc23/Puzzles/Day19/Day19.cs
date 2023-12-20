using aoc_common;

namespace aoc23.Puzzles.Day19
{
  public class Day19 : IPuzzle
  {
    public string PuzzleName => "Day 19: Aplenty";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Dictionary<string, Workflow> workflows = [];
      List<PartData> partData = [];

      string[] inputSegments = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      foreach (var inputLine in inputSegments[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        Workflow workflow = new(inputLine);
        workflows.Add(workflow.Name, workflow);
      }
      foreach (var inputLine in inputSegments[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        partData.Add(new(inputLine));
      }

      Part1(workflows, partData);
    }

    private static void Part1(Dictionary<string, Workflow> workflows, List<PartData> partData)
    {
      Console.WriteLine("Part 1:");
      long sum = 0;
      foreach (var data in partData)
      {
        Workflow? curWorkflow = workflows["in"];
        string result = "";
        while (!result.Equals("A") && !result.Equals("R") && curWorkflow != null)
        {
          result = curWorkflow.Evaluate(data);
          if (workflows.TryGetValue(result, out Workflow? newWorkflow))
          {
            curWorkflow = newWorkflow;
          }
          else
          {
            curWorkflow = null;
          }
        }
        if (result.Equals("A"))
        {
          Console.WriteLine("Accepted: " + string.Join(", ", data.Rating));
          foreach (var rating in data.Rating.Values)
          {
            sum += rating;
          }
        }
      }

      Console.WriteLine($"Sum of all accepted parts: {sum}");
    }
  }
}
