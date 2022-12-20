#undef TRACE

using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day20
{
  public class Day20 : IPuzzle
  {
    public string PuzzleName => "Day 20: Grove Positioning System";

    public string InputFileName => @"Puzzles\Day20\Day20Input.txt";

    private const long DecryptionKey = 811589153;

    public void Run(string input)
    {
      Node? zero = null;
      List<Node> buffer = new();
      int i = 0;
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        i++;
        Node node = new() { Number = int.Parse(inputLine), InitialPosition = i };
        buffer.Add(node);
        if (node.Number == 0)
        {
          zero = node;
        }
      }

      if (zero == null)
      {
        throw new ArgumentException("Input contains no zero!");
      }

      // We need to go through the buffer numbers in their original order, so keep a copy for reference.
      List<Node> original = new(buffer);
#if TRACE
      Console.WriteLine("Initial arrangement:");
      Console.WriteLine(string.Join(", ", original));
#endif

      foreach (Node moveNum in original)
      {
        MoveNumberInBuffer(buffer, moveNum);
#if TRACE
        Console.WriteLine($"Move {moveNum}.");
        Console.WriteLine(string.Join(", ", buffer));
#endif
      }
      long num1000 = FindNumOffsetFromNode(buffer, zero, 1000);
      long num2000 = FindNumOffsetFromNode(buffer, zero, 2000);
      long num3000 = FindNumOffsetFromNode(buffer, zero, 3000);
      Console.WriteLine($"The offset numbers are {num1000}, {num2000} and {num3000}, resulting in a sum of {num1000 + num2000 + num3000}.");

      // Part 2
      original.ForEach(n => n.Number *= DecryptionKey);
      buffer = new(original);

      for (int it = 1; it <= 10; it++)
      {
        Console.WriteLine($"Iteration {it}.");
        foreach (Node moveNum in original)
        {
          MoveNumberInBuffer(buffer, moveNum);
        }
      }

      num1000 = FindNumOffsetFromNode(buffer, zero, 1000);
      num2000 = FindNumOffsetFromNode(buffer, zero, 2000);
      num3000 = FindNumOffsetFromNode(buffer, zero, 3000);
      Console.WriteLine($"The offset numbers after applying the decryption key and iterating 10 times are {num1000}, {num2000} and {num3000}, resulting in a sum of {num1000 + num2000 + num3000}.");
    }

    private static void MoveNumberInBuffer(List<Node> buffer, Node moveNum)
    {
      int curIndex = buffer.IndexOf(moveNum);
      long newIndex = curIndex + moveNum.Number;

      // The count is -1 because we count the number of spaces between positions, not the number of positions.
      // There technically is only one space between the first and the last number.
      if (newIndex < 0)
      {
        // .NET doesn't have a Math.floorMod() like Java does.
        // And just subtracting the buffer count in a loop would take too long after applying the Decryption Key.
        // So use division and multiplication to apply the proper offset at once.
        long wraps = Math.Abs(moveNum.Number / (buffer.Count - 1)) + 1;
        newIndex += wraps * (buffer.Count - 1);
      }
      newIndex %= buffer.Count - 1;

      buffer.RemoveAt(curIndex);
      buffer.Insert((int)newIndex, moveNum);
    }

    private static long FindNumOffsetFromNode(List<Node> buffer, Node node, int offset)
    {
      int zeroIndex = buffer.IndexOf(node);
      int newIndex = zeroIndex + offset;
      while (newIndex < 0)
      {
        newIndex += buffer.Count;
      }
      newIndex %= buffer.Count;
      return buffer[newIndex].Number;
    }
  }
}
