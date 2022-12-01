namespace aoc22.Puzzles
{
  /// <summary>
  /// Provides a solution to a given Advent of Code puzzle.
  /// </summary>
  interface IPuzzleSolver
  {
    /// <summary>
    /// Gets the puzzle's name.
    /// </summary>
    string PuzzleName { get; }

    /// <summary>
    /// Performs the puzzle's required computation on the given input and returns the result.
    /// </summary>
    /// <param name="input">Input parameters of the puzzle.</param>
    /// <returns>Puzzle solution for the given input.</returns>
    string Compute(string input);
  }
}
