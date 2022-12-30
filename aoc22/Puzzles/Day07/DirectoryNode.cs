using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day07
{
  internal class DirectoryNode : Node
  {
    public DirectoryNode? Parent { get; }
    public ISet<Node> Children { get; set; } = new HashSet<Node>();

    public DirectoryNode(string name, DirectoryNode? parent) : base(name)
    {
      Parent = parent;
    }

    public override long Size => Children.Sum(n => n.Size);
  }
}