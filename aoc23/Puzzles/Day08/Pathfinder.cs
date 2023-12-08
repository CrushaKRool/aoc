namespace aoc23.Puzzles.Day08
{
  internal class Pathfinder(string startNode)
  {
    public long Steps { get; set; }
    public string StartNode { get; } = startNode;
    public string NextNode { get; set; } = startNode;

    public void Move(string instructions, Dictionary<string, Node> nodes)
    {
      char move = instructions[(int)(Steps % instructions.Length)];
      Node node = nodes[NextNode];
      NextNode = node.GetNextByDirection(move);
      Steps++;
    }
  }
}
