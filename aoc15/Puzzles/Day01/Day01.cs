using aoc_common;

namespace aoc15.Puzzles.Day01
{
  public class Day01 : IPuzzle
  {
    public string PuzzleName => "Day 1: Not Quite Lisp";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      int floor = 0;
      int curPos = 0;
      bool enteredBasement = false;
      foreach (char c in input)
      {
        curPos++;
        switch(c)
        {
          case '(':
            floor++;
            break;
          case ')':
            floor--;
            break;
        }
        if (floor < 0 && !enteredBasement)
        {
          Console.WriteLine($"Entering basement at position {curPos}.");
          enteredBasement = true;
        }
      }
      Console.WriteLine($"The instructions will take us to floor {floor}.");
    }
  }
}
