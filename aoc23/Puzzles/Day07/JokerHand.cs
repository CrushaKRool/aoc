namespace aoc23.Puzzles.Day07
{
  internal class JokerHand : IComparable<JokerHand>
  {
    public string Cards { get; }
    public int Bid { get; }
    public HandType HandType { get; }

    public JokerHand(string inputLine)
    {
      string[] inputParts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Cards = inputParts[0];
      Bid = int.Parse(inputParts[1]);
      HandType = DetermineHandType(Cards);
    }

    public int CompareTo(JokerHand? other)
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
      var cardSets = cards.GroupBy(c => c).OrderDescending(new GroupingComparer()).ToList();
      int numJokers = cardSets.Where(g => g.Key == 'J').Select(g => g.Count()).FirstOrDefault(0);
      for (int i = 0; i < cardSets.Count; i++)
      {
        var curSet = cardSets[i];
        bool curSetIsJoker = curSet.Key == 'J';
        int curSetCount = curSet.Count();
        if (!curSetIsJoker)
        {
          curSetCount += numJokers;
        }
        else if (i + 1 < cardSets.Count)
        {
          // Edge case: Our highest hand so far are jokers. So add the next highest cards to their count.
          // Just pray that none of the other cases becomes relevant then where we actually need to look at the next set...
          curSetCount += cardSets[i + 1].Count();
        }
        switch (curSetCount)
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
        'T' => 10,
        '9' => 9,
        '8' => 8,
        '7' => 7,
        '6' => 6,
        '5' => 5,
        '4' => 4,
        '3' => 3,
        '2' => 2,
        'J' => 1, // Individual Joker is weaker than any other card.
        _ => throw new ArgumentOutOfRangeException(nameof(card), card, "Unsupported card value!")
      };
    }

    /// <summary>
    /// Orders by grouping count, but if one of two equally sized groups contains the Jokers, it is given lower priority.
    /// That way the Jokers can be properly used to form higher-valued card hands.
    /// </summary>
    private sealed class GroupingComparer : IComparer<IGrouping<char, char>>
    {
      public int Compare(IGrouping<char, char>? x, IGrouping<char, char>? y)
      {
        if (x == null && y == null)
        {
          return 0;
        }
        else if (x != null && y == null)
        {
          return 1;
        }
        else if (x == null && y != null)
        {
          return -1;
        }
        int compResult = x.Count().CompareTo(y.Count());
        if (compResult == 0)
        {
          if (x.Key == 'J')
          {
            return -1;
          }
          if (y.Key == 'J')
          {
            return 1;
          }
        }
        return compResult;
      }
    }
  }
}
