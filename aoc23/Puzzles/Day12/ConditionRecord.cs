using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc23.Puzzles.Day12
{
  internal class ConditionRecord
  {
    public string Condition { get; set; }
    public int[] DamagedGroups { get; set; }

    public ConditionRecord(string inputLine)
    {
      string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Condition = parts[0];
      DamagedGroups = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
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
    }

    public IEnumerable<string> GetPermutationsOfCondition()
    {
      return TryPermutateCondition(Condition);
    }

    private IEnumerable<string> TryPermutateCondition(string condition)
    {
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
      List<int> groups = [];
      bool curIsDamaged = condition[0] == '#';
      int curDamagedStreak = curIsDamaged ? 1 : 0;
      for (int i = 1; i < condition.Length; i++)
      {
        bool newIsDamaged = condition[i] == '#';
        if (newIsDamaged)
        {
          curDamagedStreak++;
        }
        else
        {
          if (curIsDamaged && curDamagedStreak > 0)
          {
            groups.Add(curDamagedStreak);
            curDamagedStreak = 0;
          }
        }
        curIsDamaged = newIsDamaged;
      }
      if (curDamagedStreak > 0)
      {
        groups.Add(curDamagedStreak);
      }
      return groups.SequenceEqual(DamagedGroups);
    }
  }
}
