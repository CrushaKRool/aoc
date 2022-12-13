using aoc_common;
using System.Reflection;

namespace aoc22
{
  public static class Program
  {
    /// <summary>
    /// Main entry point for all puzzles. Selection of puzzle number is done via interactive prompt.
    /// </summary>
    /// <param name="args">Unused.</param>
    static void Main(string[] args)
    {
      CommandLineUtil.StartPuzzleViaCommandPrompt(Assembly.GetExecutingAssembly(), nameof(aoc22));
    }
  }
}
