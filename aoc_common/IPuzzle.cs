namespace aoc_common
{
  public interface IPuzzle
  {
    string PuzzleName { get; }

    string InputFileName { get; }

    void Run(string input);
  }
}
