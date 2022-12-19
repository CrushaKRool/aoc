using System.Collections.Generic;
using System.Resources;

namespace aoc22.Puzzles.Day19
{
  internal class RobotRecipe
  {
    public Mineral MiningType { get; init; }
    public IDictionary<Mineral, int> Costs { get; } = new Dictionary<Mineral, int>();

    /// <summary>
    /// Indicates whether we can afford this robot with the given resources.
    /// </summary>
    /// <param name="resources">Available resources.</param>
    /// <returns>True if we can afford it.</returns>
    public bool CanAfford(IDictionary<Mineral, int> resources)
    {
      foreach (var kvp in Costs)
      {
        resources.TryGetValue(kvp.Key, out int res);
        if (res < kvp.Value)
        {
          return false;
        }
      }
      return true;
    }

    public override string ToString()
    {
      return $"Mineral: {MiningType}; Costs: {string.Join(", ", Costs)}";
    }
  }
}
