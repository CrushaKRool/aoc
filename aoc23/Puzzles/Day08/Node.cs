namespace aoc23.Puzzles.Day08
{
  internal record Node(string Name, string Left, string Right)
  {
    public string GetNextByDirection(char direction)
    {
      if (direction == 'L')
      {
        return Left;
      }
      if (direction == 'R')
      {
        return Right;
      }
      throw new ArgumentException("Unsupported direction: " + direction, nameof(direction));
    }
  }
}
