using System.Reflection;

namespace aoc_common
{
  public static class CommandLineUtil
  {
    /// <summary>
    /// Shows a command line prompt to start a puzzle for a given day in the Advent of Code.
    /// It is expected that for a given day number XX, a class in the location "{namespace}.Puzzles.Day{XX}.Day{XX}" exists that implements IPuzzle.
    /// </summary>
    /// <param name="assemblyName">The assembly of the Advent of Code event.</param>
    /// <param name="projectNamespace">Namespace of the advent of code for which to start the puzzles.</param>
    public static void StartPuzzleViaCommandPrompt(Assembly executingAssembly, string projectNamespace)
    {
      Console.Write("Enter day number to run: ");
      string? line = Console.ReadLine();
      if (int.TryParse(line, out int dayNumber))
      {
        IPuzzle dayPuzzle;
        try
        {
          string fqn = $"{projectNamespace}.Puzzles.Day{dayNumber:D2}.Day{dayNumber:D2}";
          dayPuzzle = (IPuzzle)Activator.CreateInstance(executingAssembly.FullName, fqn).Unwrap();
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Unable to instantiate class 'Day{dayNumber:D2}'. {ex.Message}");
          return;
        }
        if (dayPuzzle == null)
        {
          Console.WriteLine($"Unable to instantiate class 'Day{dayNumber:D2}'.");
          return;
        }
        RunPuzzle(executingAssembly, dayPuzzle, dayNumber);
      }
      else
      {
        Console.WriteLine("Input must be a number.");
      }
    }

    /// <summary>
    /// Runs the given instantiated puzzle and reads the input file from the given location.
    /// </summary>
    /// <param name="executingAssembly">The assembly of the Advent of Code event.</param>
    /// <param name="puzzle">The puzzle to run.</param>
    /// <param name="dayNumber">Day number of the puzzle to run.</param>
    private static void RunPuzzle(Assembly executingAssembly, IPuzzle puzzle, int dayNumber)
    {
      Console.WriteLine();
      Console.WriteLine(puzzle.PuzzleName);
      Console.WriteLine(new string('-', puzzle.PuzzleName.Length));

      string path = Path.Combine(Path.GetDirectoryName(executingAssembly.Location) ?? "",
          "Puzzles", $"Day{dayNumber:D2}", "Data", puzzle.InputFileName);
      if (!File.Exists(path))
      {
        path = Path.Combine(Path.GetDirectoryName(executingAssembly.Location) ?? "", puzzle.InputFileName);
      }
      string input = File.ReadAllText(path);
      puzzle.Run(input);
    }
  }
}