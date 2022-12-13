using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day04
{
  /// <summary>
  /// Represents a range between a given minimum and maximum value.
  /// </summary>
  internal sealed class Range
  {
    public int Min { get; }
    public int Max { get; }

    public Range(int min, int max)
    {
      if (min <= max)
      {
        Min = min;
        Max = max;
      }
      else
      {
        Min = max;
        Max = min;
      }
    }

    /// <summary>
    /// Indicates whether this range completely contains the given other range.
    /// </summary>
    /// <param name="other">Other range to compare this one to.</param>
    /// <returns>true, if this range fully contains the other.</returns>
    public bool ContainsOther(Range other)
    {
      return Min <= other.Min && Max >= other.Max;
    }

    /// <summary>
    /// Indicates whether this range overlaps the given other range.
    /// </summary>
    /// <param name="other">Other range to compare this one to.</param>
    /// <returns>true, if this range overlaps the other.</returns>
    public bool OverlapsOther(Range other)
    {
      return Min <= other.Min && Max >= other.Min || Min <= other.Max && Max >= other.Max;
    }
  }
}
