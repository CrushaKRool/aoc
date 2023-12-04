using System.ComponentModel.Design;

namespace aoc23.Puzzles.Day04
{
  internal class Card
  {
    public int ID { get; }
    public ISet<int> WinningNumbers { get; }
    public int[] OwnNumbers { get; }
    public int OwnWinningNumberCount { get; }

    public int Count { get; set; } = 1;

    public Card(string input)
    {
      string[] inputParts = input.Split(new char[] { ':', '|' }, StringSplitOptions.TrimEntries & StringSplitOptions.RemoveEmptyEntries);
      ID = int.Parse(inputParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
      WinningNumbers = inputParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
      OwnNumbers = inputParts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
      OwnWinningNumberCount = OwnNumbers.Count(n => WinningNumbers.Contains(n));
    }

    public int GetPoints()
    {
      return OwnWinningNumberCount switch
      {
        0 => 0,
        1 => 1,
        _ => (int) Math.Pow(2, OwnWinningNumberCount - 1),
      };
    }
  }
}
