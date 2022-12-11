using aoc15.Puzzles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace aoc15
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.Write("Enter day number to run: ");
      string? line = Console.ReadLine();
      if (int.TryParse(line, out int dayNumber))
      {
        IPuzzle dayPuzzle;
        try
        {
          string fqn = $"aoc15.Puzzles.Day{dayNumber:D2}.Day{dayNumber:D2}";
          dayPuzzle = (IPuzzle)Activator.CreateInstance(null, fqn).Unwrap();
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Unable to instantiate class 'Day{dayNumber:D2}'. {ex.Message}");
          return;
        }
        if (dayPuzzle == null)
        {
          Console.WriteLine($"Unable to instantiate class 'Day{dayNumber}'.");
          return;
        }
        RunPuzzle(dayPuzzle);
      }
      else
      {
        Console.WriteLine("Input must be a number.");
      }
    }

    private static void RunPuzzle(IPuzzle puzzle)
    {
      string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), puzzle.GetInputFileName());
      string input = File.ReadAllText(path);
      puzzle.Run(input);
    }
  }
}
