using System;
using System.Collections.Generic;

namespace aoc22.Puzzles.Day16
{
  internal class ShortestPath
  {
    public string Start { get; set; } = "";
    public string End { get; set; } = "";
    public List<Node> PathNodes { get; set; } = new();

    public int GetEstimatedValue(int remainingTime, Node curPos)
    {
      int totalValue = 0;
      int startIndex = PathNodes.IndexOf(curPos) + 1;
      for (int i = startIndex; i < PathNodes.Count; i++)
      {
        remainingTime--;
        if (remainingTime <= 0)
        {
          return totalValue;
        }
        Node intermediate = PathNodes[i];
        if (!intermediate.ValveOpened && intermediate.FlowRate > 0)
        {
          remainingTime--;
          if (remainingTime <= 0)
          {
            return totalValue;
          }
          totalValue += Math.Max(remainingTime - 1, 0) * intermediate.FlowRate;
        }
      }
      return totalValue;
    }
  }
}
