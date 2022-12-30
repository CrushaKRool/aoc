#undef DEBUG

using aoc_common;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day22
{
  public class Day22 : IPuzzle
  {
    public string PuzzleName => "Day 22: Monkey Map";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      string[] split = input.Split(Environment.NewLine + Environment.NewLine);
      Board board = new(split[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries));

      char[] startRow = board.Grid[0];
      int startX = 0;
      for (startX = 0; startX < startRow.Length; startX++)
      {
        if (startRow[startX] != ' ')
        {
          break;
        }
      }
      board.PlayerX = startX;
      board.PlayerY = 0;
      board.PlayerFacing = Direction.Right;

      board.PrintBoard();

      string[] instructionSplit = Regex.Split(split[1].Trim(), "([LR]|\\d+)").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
      foreach (string instruction in instructionSplit)
      {
        if (char.IsLetter(instruction[0]))
        {
          board.RotatePlayer(instruction[0]);
        }
        else
        {
          board.MovePlayer(int.Parse(instruction));
        }
#if DEBUG
        board.PrintBoard();
#endif
      }

      board.PrintBoard();

      Console.WriteLine($"The player's position is ({board.PlayerX + 1},{board.PlayerY + 1}), facing {board.PlayerFacing}.");
      long password = ((board.PlayerY + 1) * 1000) + ((board.PlayerX + 1) * 4) + board.PlayerFacing.Value;
      Console.WriteLine($"The password is {password}.");
    }
  }
}
