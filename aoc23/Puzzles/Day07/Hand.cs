namespace aoc23.Puzzles.Day07
{
  internal class Hand : IComparable<Hand>
  {
    public string Cards { get; }
    public int Bid { get; }
    public HandType HandType { get; }

    public Hand(string inputLine)
    {
      string[] inputParts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Cards = inputParts[0];
      Bid = int.Parse(inputParts[1]);
      HandType = DetermineHandType(Cards);
    }

    public int CompareTo(Hand? other)
    {
      if (other == null)
      {
        return 1;
      }
      if (HandType > other.HandType)
      {
        return 1;
      }
      else if (HandType < other.HandType)
      {
        return -1;
      }
      for (int i = 0; i < Cards.Length && i < other.Cards.Length; i++)
      {
        int compResult = MapCardToNumber(Cards[i]).CompareTo(MapCardToNumber(other.Cards[i]));
        if (compResult != 0)
        {
          return compResult;
        }
      }
      return 0;
    }

    private static HandType DetermineHandType(string cards)
    {
      var cardSets = cards.GroupBy(c => c).OrderByDescending(g => g.Count()).ToList();
      for (int i = 0; i < cardSets.Count; i++)
      {
        int setCount = cardSets[i].Count();
        switch (setCount)
        {
          case 5:
            return HandType.FIVE_OF_A_KIND;
          case 4:
            return HandType.FOUR_OF_A_KIND;
          case 3:
            if (i + 1 < cardSets.Count && cardSets[i + 1].Count() == 2)
            {
              return HandType.FULL_HOUSE;
            }
            return HandType.THREE_OF_A_KIND;
          case 2:
            if (i + 1 < cardSets.Count && cardSets[i + 1].Count() == 2)
            {
              return HandType.TWO_PAIR;
            }
            return HandType.ONE_PAIR;
          case 1:
            return HandType.HIGH_CARD;
        }
      }
      return HandType.HIGH_CARD;
    }

    private static int MapCardToNumber(char card)
    {
      return card switch
      {
        'A' => 20,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        '9' => 9,
        '8' => 8,
        '7' => 7,
        '6' => 6,
        '5' => 5,
        '4' => 4,
        '3' => 3,
        '2' => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(card), card, "Unsupported card value!")
      };
    }
  }
}
