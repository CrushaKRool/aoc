using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day21
{
  internal class Monkey
  {
    private static readonly Regex NumberMatcher = new("(\\d+)");
    private static readonly Regex OperationMatcher = new("(\\w+) ([+\\-*/=]) (\\w+)");

    public string Name { get; init; }
    public string JobExpression { get; set; }

    private long? _cachedResult;

    public Monkey(string name, string jobExpression)
    {
      Name = name;
      JobExpression = jobExpression;
    }

    public long GetYelledNumber(IDictionary<string, Monkey> allMonkeys)
    {
      if (_cachedResult.HasValue)
      {
        return _cachedResult.Value;
      }
      if (NumberMatcher.IsMatch(JobExpression))
      {
        _cachedResult = long.Parse(JobExpression);
        return _cachedResult.Value;
      }
      Match m = OperationMatcher.Match(JobExpression);
      if (m.Success)
      {
        string nameMonkeyA = m.Groups[1].Value;
        string nameMonkeyB = m.Groups[3].Value;
        string operatorName = m.Groups[2].Value;
        long resultMonkeyA = allMonkeys[nameMonkeyA].GetYelledNumber(allMonkeys);
        long resultMonkeyB = allMonkeys[nameMonkeyB].GetYelledNumber(allMonkeys);

        if ("=" == operatorName)
        {
          // For the root monkey of part 2.
          // Don't cache anything. Just give us a delta to see if they are the same or by how much they differ.
          return resultMonkeyA - resultMonkeyB;
        }

        _cachedResult = operatorName switch
        {
          "+" => resultMonkeyA + resultMonkeyB,
          "-" => resultMonkeyA - resultMonkeyB,
          "*" => resultMonkeyA * resultMonkeyB,
          "/" => (long?)(resultMonkeyA / resultMonkeyB),
          _ => throw new ArgumentException($"Invalid operator: {operatorName}"),
        };
        return _cachedResult.Value;
      }
      throw new ArgumentException($"Unknown job expression: {JobExpression}");
    }

    public void ResetCache()
    {
      _cachedResult = null;
    }
  }
}
