using System.Text;
using System.Text.RegularExpressions;

namespace aoc23.Puzzles.Day12
{
  internal class ConditionRecord
  {
    public string Condition { get; set; }
    public int[] DamagedGroups { get; set; }

    private Regex WildcardMatchRegex { get; set; }
    private Regex FullMatchRegex { get; set; }

    public ConditionRecord(string inputLine)
    {
      string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Condition = parts[0];
      DamagedGroups = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
      InitRegex(DamagedGroups);
    }

    private void InitRegex(int[] damagedGroups)
    {
      // Wildcard Regex
      StringBuilder sb = new();
      sb.Append(@"^[.?]*");
      for (int i = 0; i < damagedGroups.Length; i++)
      {
        sb.Append(@"[#?]{" + damagedGroups[i] +"}");
        if (i < damagedGroups.Length - 1)
        {
          sb.Append(@"[.?]+");
        }
      }
      sb.Append(@"[.?]*$");
      WildcardMatchRegex = new Regex(sb.ToString());

      // Full Match Regex
      sb = new();
      sb.Append(@"^[.]*");
      for (int i = 0; i < damagedGroups.Length; i++)
      {
        sb.Append(@"[#]{" + damagedGroups[i] + "}");
        if (i < damagedGroups.Length - 1)
        {
          sb.Append(@"[.]+");
        }
      }
      sb.Append(@"[.]*$");
      FullMatchRegex = new Regex(sb.ToString());
    }

    public void Unfold()
    {
      Condition = Condition + '?' + Condition + '?' + Condition + '?' + Condition + '?' + Condition;

      List<int> newGroups = [];
      newGroups.AddRange(DamagedGroups);
      newGroups.AddRange(DamagedGroups);
      newGroups.AddRange(DamagedGroups);
      newGroups.AddRange(DamagedGroups);
      newGroups.AddRange(DamagedGroups);
      DamagedGroups = [.. newGroups];
      InitRegex(DamagedGroups);
    }

    public IEnumerable<string> GetPermutationsOfCondition()
    {
      return TryPermutateCondition(Condition);
    }

    private IEnumerable<string> TryPermutateCondition(string condition)
    {
      if (!WildcardMatchRegex.IsMatch(condition))
      {
        yield break;
      }

      int wildcardIndex = condition.IndexOf('?');
      if (wildcardIndex == -1)
      {
        // If the input contains no more wildcards, we have reached the base case of the recursion.
        yield return condition;
      }
      else
      {
        // Otherwise replace the first encountered wildcard with both possible substitutions and recurse.
        List<string> result = [];
        StringBuilder sb = new(condition);
        sb[wildcardIndex] = '.';
        result.AddRange(TryPermutateCondition(sb.ToString()));
        sb[wildcardIndex] = '#';
        result.AddRange(TryPermutateCondition(sb.ToString()));
        foreach (var r in result)
        {
          yield return r;
        }
      }
    }

    public bool ConditionMatchesDamagedGroups(string condition)
    {
      return FullMatchRegex.IsMatch(condition);
    }
  }
}
