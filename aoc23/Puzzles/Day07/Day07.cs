using aoc_common;

namespace aoc23.Puzzles.Day07
{
  public class Day07 : IPuzzle
  {
    public string PuzzleName => "Day 7: Camel Cards";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      Part1(input);

      Console.WriteLine("-------------------------------------------------------------");

      Part2(input);
    }

    static void Part1(string input)
    {
      Console.WriteLine("Part 1:");

      List<Hand> hands = [];
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        hands.Add(new Hand(inputLine));
      }

      long winnings = 0;
      int i = 1;
      foreach (var hand in hands.Order())
      {
        Console.WriteLine($"Rank {i}: {hand.Cards} [{hand.HandType}] with bid {hand.Bid}");
        winnings += hand.Bid * i;
        i++;
      }
      Console.WriteLine($"Total winnings: {winnings}");
    }

    static void Part2(string input)
    {
      Console.WriteLine("Part 2:");

      List<JokerHand> hands = [];
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        hands.Add(new JokerHand(inputLine));
      }

      long winnings = 0;
      int i = 1;
      foreach (var hand in hands.Order())
      {
        Console.WriteLine($"Rank {i}: {hand.Cards} [{hand.HandType}] with bid {hand.Bid}");
        winnings += hand.Bid * i;
        i++;
      }
      Console.WriteLine($"Total winnings: {winnings}");
    }
  }
}
