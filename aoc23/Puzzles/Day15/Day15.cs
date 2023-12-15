using aoc_common;
using System.Reflection.Emit;

namespace aoc23.Puzzles.Day15
{
  public class Day15 : IPuzzle
  {
    public string PuzzleName => "Day 15: Lens Library";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);
      Part2(input);
    }

    private static void Part1(string input)
    {
      Console.WriteLine("Part 1:");
      long sum = 0;
      foreach (string step in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        sum += Hash(step);
      }
      Console.WriteLine($"Sum of hashes: {sum}");
    }

    private static void Part2(string input)
    {
      Console.WriteLine("Part 2:");
      LinkedList<Lens>[] boxes = new LinkedList<Lens>[256];
      for (int i = 0; i < boxes.Length; i++)
      {
        boxes[i] = new();
      }

      foreach (string step in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        if (step.EndsWith('-'))
        {
          RemoveLens(boxes, step);
        }
        else
        {
          AddLens(boxes, step);
        }
      }

      long power = 0;
      for (int i = 0; i < boxes.Length; i++)
      {
        LinkedList<Lens> box = boxes[i];
        int j = 0;
        foreach (var lens in box)
        {
          power += (i + 1) * (j + 1) * lens.FocalLength;
          j++;
        }
      }
      Console.WriteLine($"Total focusing power: {power}");
    }

    private static void RemoveLens(LinkedList<Lens>[] boxes, string step)
    {
      string label = step[..^1];
      int hash = Hash(label);
      LinkedList<Lens> box = boxes[hash];
      LinkedListNode<Lens>? node = box.First;
      while (node != null)
      {
        var next = node.Next;
        if (node.Value.Label.Equals(label))
        {
          box.Remove(node);
          break;
        }
        node = next;
      }
    }

    private static void AddLens(LinkedList<Lens>[] boxes, string step)
    {
      string[] parts = step.Split('=');
      Lens lens = new(parts[0], int.Parse(parts[1]));
      int hash = Hash(lens.Label);
      LinkedList<Lens> box = boxes[hash];
      LinkedListNode<Lens>? node = box.First;
      bool existingNode = false;
      while (node != null)
      {
        var next = node.Next;
        if (node.Value.Label.Equals(lens.Label))
        {
          node.Value = lens;
          existingNode = true;
          break;
        }
        node = next;
      }
      if (!existingNode)
      {
        box.AddLast(lens);
      }
    }

    private static int Hash(string input)
    {
      int hash = 0;
      foreach (char c in input)
      {
        hash += c;
        hash *= 17;
        hash %= 256;
      }
      return hash;
    }
  }
}
