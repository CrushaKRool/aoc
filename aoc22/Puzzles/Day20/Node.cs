using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day20
{
  /// <summary>
  /// Since the input numbers may contain duplicates, we need this wrapper class that also contains their initial position
  /// to create a unique object that we can identify in the list as we keep moving stuff around.
  /// </summary>
  internal class Node
  {
    public long Number { get; set; }
    public int InitialPosition { get; init; }

    public override bool Equals(object? obj)
    {
      return obj is Node node &&
             Number == node.Number &&
             InitialPosition == node.InitialPosition;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Number, InitialPosition);
    }

    public override string ToString()
    {
      return $"{Number} ({InitialPosition})";
    }
  }
}
