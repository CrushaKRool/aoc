using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day16
{
  internal class PathPair
  {
    public ShortestPath PathA { get; set; }
    public ShortestPath PathB { get; set; }

    public PathPair(ShortestPath pathA, ShortestPath pathB)
    {
      PathA = pathA;
      PathB = pathB;
    }

    public int GetCombinedValue(int remainingTime, Node curA, Node curB)
    {
      return PathA.GetEstimatedValue(remainingTime, curA) + PathB.GetEstimatedValue(remainingTime, curB);
    }
  }
}
