using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aoc15.Puzzles.Day04
{
  public class Day04 : IPuzzle
  {
    public string PuzzleName => "Day 4: The Ideal Stocking Stuffer";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      int i = 0;
      string hash;
      do
      {
        i++;
        byte[] inputBytes = Encoding.ASCII.GetBytes(input + i);
        byte[] hashBytes = MD5.HashData(inputBytes);
        hash = Convert.ToHexString(hashBytes);
      } while (!hash.StartsWith("00000"));

      Console.WriteLine($"The positive number {i} results in the MD5 hash {hash}, which starts with five zeros.");

      do
      {
        i++;
        byte[] inputBytes = Encoding.ASCII.GetBytes(input + i);
        byte[] hashBytes = MD5.HashData(inputBytes);
        hash = Convert.ToHexString(hashBytes);
      } while (!hash.StartsWith("000000"));

      Console.WriteLine($"The positive number {i} results in the MD5 hash {hash}, which starts with six zeros.");
    }
  }
}
