namespace aoc22.Puzzles.Day07
{
  internal class FileNode : Node
  {
    private readonly long _size;

    public FileNode(string name, long size) : base(name)
    {
      _size = size;
    }

    public override long Size => _size;
  }
}