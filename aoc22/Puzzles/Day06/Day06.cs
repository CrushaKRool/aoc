using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc22.Puzzles.Day06
{
  public class Day06 : IPuzzle
  {
    public string PuzzleName => "Day 6: Tuning Trouble";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Console.WriteLine(FindStartMarker(input.Trim(), 4));
      Console.WriteLine(FindStartMarker(input.Trim(), 14));
    }

    /// <summary>
    /// Returns a string describing the found start marker in the given input string.
    /// </summary>
    /// <param name="input">Input in which to find the start marker.</param>
    /// <param name="markerLength">Number of consecutive distinct characters that indicate the marker start.</param>
    /// <returns>String describing the result.</returns>
    private static string FindStartMarker(string input, int markerLength)
    {
      LimitedQueue<char> chars = new(markerLength);
      int numCharsProcessed = 0;
      foreach (char c in input)
      {
        numCharsProcessed++;
        chars.Enqueue(c);
        // Are all characters in the queue different from each other?
        if (chars.Distinct().Count() == markerLength)
        {
          return $"Found start marker \"{string.Join("", chars)}\" after processing {numCharsProcessed} characters.";
        }
      }

      return "No marker found.";
    }

    public class LimitedQueue<T> : Queue<T>
    {
      public int Limit { get; set; }

      public LimitedQueue(int limit) : base(limit)
      {
        Limit = limit;
      }

      public new void Enqueue(T item)
      {
        while (Count >= Limit)
        {
          Dequeue();
        }
        base.Enqueue(item);
      }
    }
  }
}