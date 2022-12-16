using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day16
{
  internal class Node
  {
    public string Name { get; }
    public int FlowRate { get; }
    public bool ValveOpened { get; set; }
    public List<string> TunnelsTo { get; } = new();
    public List<ShortestPath> ShortestPaths { get; } = new();

    public Node(string name, int flowRate, params string[] tunnels)
    {
      Name = name;
      FlowRate = flowRate;
      TunnelsTo.AddRange(tunnels);
    }
  }
}
