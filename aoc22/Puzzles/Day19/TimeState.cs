using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day19
{
  /// <summary>
  /// Represents the value of a blueprint at a fixed point in time.
  /// </summary>
  internal class TimeState
  {
    private readonly Blueprint _blueprint;

    public TimeState? PreviousState { get; init; }
    public List<TimeState> NextStates { get; } = new();

    /// <summary>
    /// Which action was the taken to get to this state. If null, none was taken that turn.
    /// </summary>
    public Mineral? ActionTaken { get; init; }
    public int MinutesRemaining { get; init; }

    public IDictionary<Mineral, int> Resources { get; } = new Dictionary<Mineral, int>();
    public IDictionary<Mineral, int> Robots { get; } = new Dictionary<Mineral, int>();

    public TimeState(TimeState? previous, Mineral? action, int minute, Blueprint blueprint)
    {
      PreviousState = previous;
      ActionTaken = action;
      MinutesRemaining = minute;
      _blueprint = blueprint;

      if (previous != null)
      {
        foreach (var kvp in previous.Resources)
        {
          Resources[kvp.Key] = kvp.Value;
        }
        foreach (var kvp in previous.Robots)
        {
          Robots[kvp.Key] = kvp.Value;
        }
      }
      else
      {
        Resources[Mineral.Ore] = 0;
        Resources[Mineral.Clay] = 0;
        Resources[Mineral.Obsidian] = 0;
        Resources[Mineral.Geode] = 0;
        Robots[Mineral.Ore] = 1; // Starting with 1 ore robot.
        Robots[Mineral.Clay] = 0;
        Robots[Mineral.Obsidian] = 0;
        Robots[Mineral.Geode] = 0;
      }
    }

    public IEnumerable<TimeState> GetLeafStates()
    {
      if (NextStates.Count == 0)
      {
        yield return this;
      }
      else
      {
        foreach (TimeState child in NextStates)
        {
          foreach (var leaf in child.GetLeafStates())
          {
            yield return leaf;
          }
        }
      }
    }

    public void PopulateNextStates()
    {
      if (MinutesRemaining <= 0)
      {
        return;
      }

      foreach (Mineral? option in GetNextOptions())
      {
        NextStates.Add(CreateStateForOption(option));
      }

      foreach (TimeState state in NextStates)
      {
        state.PopulateNextStates();
      }
    }

    private IEnumerable<Mineral?> GetNextOptions()
    {
      List<Mineral?> result = new()
      {
        null // We can always just wait.
      };

      foreach (RobotRecipe recipe in _blueprint.RobotRecipes)
      {
        if (!recipe.CanAfford(Resources))
        {
          continue;
        }
        if (ActionTaken == null && PreviousState != null && recipe.CanAfford(PreviousState.Resources))
        {
          // Branch pruning optimization:
          // If we could have build the robot already last turn and decided to do nothing, it's not worth building it this turn either.
          // We could have gotten more resources out of building it last turn, after all.
          continue;
        }
        int maxResConsumption = GetMaxResourceConsumption(recipe.MiningType);
        if (Robots[recipe.MiningType] + 1 > maxResConsumption)
        {
          // Branch pruning optimization:
          // We already have enough robots to mine the maximum needed amount of that resource per round,
          // so no need to build any more of this type.
          continue;
        }
        result.Add(recipe.MiningType);
      }

      if (result.Contains(Mineral.Geode))
      {
        // Branch pruning optimization:
        // If we could build a geode miner, ignore all other options. We want to maximize geodes.
        return new Mineral?[] { Mineral.Geode };
      }

      //if (result.Contains(Mineral.Obsidian))
      //{
      //  // Branch pruning optimization:
      //  // If we could build an obsidian miner, either build it or save the resources for a potential geode miner.
      //  // We need obsidian to mine geodes and we want it as soon as possible, so don't waste it on other miners.
      //  return new Mineral?[] { Mineral.Obsidian, null };
      //}

      return result;
    }

    private int GetMaxResourceConsumption(Mineral resourceType)
    {
      if (resourceType == Mineral.Geode)
      {
        return int.MaxValue; // We can never have enough geodes.
      }
      return _blueprint.RobotRecipes.Select(r => r.Costs.ContainsKey(resourceType) ? r.Costs[resourceType] : 0).Max();
    }

    private TimeState CreateStateForOption(Mineral? option)
    {
      // We always get a new state with the given action and mine our current resources.
      TimeState nextState = new TimeState(this, option, MinutesRemaining - 1, _blueprint);
      MineResources(nextState);
      if (option != null)
      {
        BuildRobot(nextState, _blueprint.RobotRecipes.First(r => r.MiningType == option));
      }
      return nextState;
    }

    private void MineResources(TimeState nextState)
    {
      foreach (var kvpMiner in Robots)
      {
        nextState.Resources[kvpMiner.Key] += kvpMiner.Value;
      }
    }

    private void BuildRobot(TimeState nextState, RobotRecipe recipe)
    {
      foreach (var kvpRes in recipe.Costs)
      {
        nextState.Resources[kvpRes.Key] -= kvpRes.Value;
      }
      nextState.Robots[recipe.MiningType]++;
    }

    public override string ToString()
    {
      return $"Minutes Remaining: {MinutesRemaining}; Action: {ActionTaken}; Res: {string.Join(", ", Resources)}; Miners: {string.Join(", ", Robots)}";
    }
  }
}
