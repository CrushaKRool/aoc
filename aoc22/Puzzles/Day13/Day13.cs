using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace aoc22.Puzzles.Day13
{
  public class Day13 : IPuzzle
  {
    public string PuzzleName => "Day 13: Distress Signal";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<string> inputLines = input.Split(Environment.NewLine).Where(l => !string.IsNullOrEmpty(l)).ToList();
      long indexSum = 0;
      for (int i = 0; i < inputLines.Count - 1; i += 2)
      {
        int curIndex = i/2 + 1;
        // All inputs are basically JSON arrays, so let the built-in parser do the heavy lifting.
        JsonArray packageA = (JsonArray)JsonArray.Parse(inputLines[i]);
        JsonArray packageB = (JsonArray)JsonArray.Parse(inputLines[i+1]);
        int compareResult = CompareNested(packageA, packageB);
        if (compareResult == -1)
        {
          Console.WriteLine($"Packages at index {curIndex} are in order.");
          indexSum += curIndex;
        }
        else
        {
          Console.WriteLine($"Packages at index {curIndex} are out of order.");
        }
      }

      Console.WriteLine($"The total sum of indices with ordered packages is {indexSum}.");

      // Part 2
      string dividerA = "[[2]]";
      string dividerB = "[[6]]";
      inputLines.Add(dividerA);
      inputLines.Add(dividerB);
      inputLines.Sort((a, b) =>
      {
        JsonArray packageA = (JsonArray)JsonArray.Parse(a);
        JsonArray packageB = (JsonArray)JsonArray.Parse(b);
        return CompareNested(packageA, packageB);
      });

      int dividerAIndex = inputLines.IndexOf(dividerA) + 1;
      int dividerBIndex = inputLines.IndexOf(dividerB) + 1;

      Console.WriteLine();
      Console.WriteLine($"The decoder key is {dividerAIndex * dividerBIndex}.");
    }

    private int CompareNested(JsonArray a, JsonArray b)
    {
      if(a.Count == 0)
      {
        if (b.Count == 0)
        {
          return 0;
        }
        return -1;
      }
      else if (b.Count == 0)
      {
        return 1;
      }
      for (int i = 0; i < Math.Max(a.Count, b.Count); i++)
      {
        if (i < b.Count && i >= a.Count)
        {
          return -1;
        }
        else if (i < a.Count && i >= b.Count)
        {
          return 1;
        }
        if (a[i] is JsonArray || b[i] is JsonArray)
        {
          JsonArray arrayA = GetElemAsArray(a, i);
          JsonArray arrayB = GetElemAsArray(b, i);
          int compareResult = CompareNested(arrayA, arrayB);
          if (compareResult != 0)
          {
            return compareResult;
          }
        }
        else
        {
          int valA = a[i].GetValue<int>();
          int valB = b[i].GetValue<int>();
          int compareResult = valA.CompareTo(valB);
          if (compareResult != 0)
          {
            return compareResult;
          }
        }
      }
      return 0;
    }

    private JsonArray GetElemAsArray(JsonArray source, int sourceIndex)
    {
      JsonNode? node = source[sourceIndex];
      if (node is JsonArray array)
      {
        return array;
      }
      else if (node != null)
      {
        return new JsonArray() { node.GetValue<int>() };
      }
      else
      {
        throw new ArgumentException("No node at given source index.");
      }
    }
  }
}
