using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day25
{
  internal class SnafuConverter
  {
    public static long FromSnafu(string snafu)
    {
      int[] digits = new int[snafu.Length];

      for (int i = 0; i < snafu.Length; i++)
      {
        char c = snafu[snafu.Length - 1 - i];
        digits[i] = c switch
        {
          '=' => -2,
          '-' => -1,
          '0' => 0,
          '1' => 1,
          '2' => 2,
          _ => throw new ArgumentException($"Unknown snafu digit: {c}")
        };
      }

      long result = 0;
      for (int i = 0; i < digits.Length; i++)
      {
        result += digits[i] * (long)Math.Pow(5, i);
      }
      return result;
    }

    public static string ToSnafu(long number)
    {
      StringBuilder sb = new();

      while (number != 0)
      {
        long digitIndex = (number + 2) % 5;
        sb.Insert(0, digitIndex switch
        {
          0 => '=',
          1 => '-',
          2 => '0',
          3 => '1',
          4 => '2',
          _ => new ArgumentException($"Unexpected index: {digitIndex}"),
        });
        number = (number + 2) / 5;
      }

      return sb.ToString();
    }
  }
}
