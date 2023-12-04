using aoc_common;

namespace aoc23.Puzzles.Day04
{
  public class Day04 : IPuzzle
  {
    public string PuzzleName => "Day 4: Scratchcards";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      List<Card> cards = [];
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
      {
        cards.Add(new Card(inputLine));
      }

      Part1(cards);

      Console.WriteLine("-----------------------------------------------------");

      Part2(cards);
    }

    private static void Part1(List<Card> cards)
    {
      Console.WriteLine("Part 1:");
      int sum = 0;
      foreach (Card card in cards)
      {
        int points = card.GetPoints();
        Console.WriteLine($"Card {card.ID} is worth {points}.");
        sum += points;
      }
      Console.WriteLine($"Sum of card points is {sum}.");
    }

    private static void Part2(List<Card> cards)
    {
      Console.WriteLine("Part 2:");
      for (int i = 0; i < cards.Count; i++)
      {
        Card curCard = cards[i];
        Console.WriteLine($"Card {curCard.ID} has {curCard.Count} instances.");
        int winCount = curCard.OwnWinningNumberCount;
        for (int j = i + 1; j < i + 1 + winCount && j < cards.Count; j++)
        {
          cards[j].Count += curCard.Count;
        }
      }

      long cardCount = cards.Sum(c => c.Count);
      Console.WriteLine($"Card count: {cardCount}");
    }
  }
}
