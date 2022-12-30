using aoc_common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc15.Puzzles.Day03
{
  public class Day03 : IPuzzle
  {
    public string PuzzleName => "Day 3: Perfectly Spherical Houses in a Vacuum";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      IDictionary<Point, int> deliveredPresents = new Dictionary<Point, int>();
      DeliverPresents(deliveredPresents, input.Trim(), 0, 1);

      Console.WriteLine($"{deliveredPresents.Count} houses receive at least one present.");

      // Part 2

      deliveredPresents.Clear();
      DeliverPresents(deliveredPresents, input.Trim(), 0, 2);
      DeliverPresents(deliveredPresents, input.Trim(), 1, 2);

      Console.WriteLine($"With two santas, {deliveredPresents.Count} houses receive at least one present.");
    }

    private static void DeliverPresents(IDictionary<Point, int> deliveredPresents, string input,
      int inputStartOffset, int inputStepSize)
    {
      int curX = 0;
      int curY = 0;

      deliveredPresents[new Point(curX, curY)] = 1;

      for (int i = inputStartOffset; i < input.Length; i += inputStepSize)
      {
        char c = input[i];

        switch (c)
        {
          case '^':
            curY--;
            break;
          case '>':
            curX++;
            break;
          case '<':
            curX--;
            break;
          case 'v':
            curY++;
            break;
          default:
            throw new ArgumentException($"Unexpected move: {c}");
        }

        Point curHouse = new(curX, curY);
        if (deliveredPresents.TryGetValue(curHouse, out int visitCount))
        {
          deliveredPresents[curHouse] = visitCount + 1;
        }
        else
        {
          deliveredPresents[curHouse] = 1;
        }
      }
    }
  }
}
