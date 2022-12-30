namespace aoc22.Puzzles.Day12
{
  internal sealed class Node
  {
    public char Elevation { get; }
    public int NumericElevation { get; }
    public int X { get; }
    public int Y { get; }
    public int DistanceFromStart { get; set; }
    public Node? PreviousNode { get; set; }

    public Node(char elevation, int x, int y)
    {
      X = x;
      Y = y;
      Elevation = elevation;
      if (elevation == 'S')
      {
        NumericElevation = 'a' - 'a';
      }
      else if (elevation == 'E')
      {
        NumericElevation = 'z' - 'a';
      }
      else
      {
        NumericElevation = elevation - 'a';
      }
    }

    public bool IsReachableFromElevation(int OtherElevation)
    {
      return NumericElevation - OtherElevation < 2;
    }

    public override string ToString()
    {
      return $"Node: {Elevation}; Distance: {DistanceFromStart}; ({X},{Y})";
    }
  }
}