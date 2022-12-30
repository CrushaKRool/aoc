using System;

namespace aoc22.Puzzles.Day07
{
  internal abstract class Node
  {
    public string Name { get; }
    public virtual long Size { get; }

    protected Node(string name)
    {
      Name = name;
    }

    public override bool Equals(object? obj)
    {
      return obj is Node node &&
             Name == node.Name;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Name);
    }

    public override string ToString()
    {
      return Name;
    }
  }
}