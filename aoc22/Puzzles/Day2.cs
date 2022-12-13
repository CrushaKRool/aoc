using System;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles
{
  class Day2 : IPuzzleSolver
  {
    private const int ScoreLoss = 0;
    private const int ScoreDraw = 3;
    private const int ScoreWin = 6;

    private readonly Regex LineParser = new("([ABC]) ([XYZ])");

    public string PuzzleName => "Day 2: Rock Paper Scissors";

    public string SolvePart1(string input)
    {
      int score = 0;
      foreach (string line in input.Split(Environment.NewLine))
      {
        Match match = LineParser.Match(line);
        if (match.Success)
        {
          IHand opponent = GetHandByLetter(match.Groups[1].Value);
          IHand self = GetHandByLetter(match.Groups[2].Value);
          RPSOutcome battleOutcome = self.GetBattleOutcome(opponent);
          switch(battleOutcome)
          {
            case RPSOutcome.Win:
              score += ScoreWin;
              break;
            case RPSOutcome.Draw:
              score += ScoreDraw;
              break;
            case RPSOutcome.Loss:
              score += ScoreLoss;
              break;
          }
          score += self.ScoreHand;
        }
      }
      return $"Executing the given unclear strategy would result in a score of {score}.";
    }

    public string SolvePart2(string input)
    {
      int score = 0;
      foreach (string line in input.Split(Environment.NewLine))
      {
        Match match = LineParser.Match(line);
        if (match.Success)
        {
          IHand opponent = GetHandByLetter(match.Groups[1].Value);
          RPSOutcome desiredOutcome = GetOutcomeByLetter(match.Groups[2].Value);
          IHand self = opponent.GetHandForDesiredOutcome(desiredOutcome);
          switch (desiredOutcome)
          {
            case RPSOutcome.Win:
              score += ScoreWin;
              break;
            case RPSOutcome.Draw:
              score += ScoreDraw;
              break;
            case RPSOutcome.Loss:
              score += ScoreLoss;
              break;
          }
          score += self.ScoreHand;
        }
      }
      return $"Executing the given decrypted strategy would result in a score of {score}.";
    }

    private IHand GetHandByLetter(string letter)
    {
      return letter switch
      {
        "A" => Rock.Singleton,
        "B" => Paper.Singleton,
        "C" => Scissors.Singleton,
        "X" => Rock.Singleton,
        "Y" => Paper.Singleton,
        "Z" => Scissors.Singleton,
        _ => throw new ArgumentException($"Unknown RPS letter: {letter}")
      };
    }

    private RPSOutcome GetOutcomeByLetter(string letter)
    {
      return letter switch
      {
        "X" => RPSOutcome.Loss,
        "Y" => RPSOutcome.Draw,
        "Z" => RPSOutcome.Win,
        _ => throw new ArgumentException($"Unknown Outcome letter: {letter}")
      };
    }

    /// <summary>
    /// Represents the possible hands. Just to get a little more type-safety.
    /// </summary>
    private enum RPSHands { Rock, Paper, Scissors };

    /// <summary>
    /// Represents the possible outcomes of a battle.
    /// </summary>
    private enum RPSOutcome { Win, Loss, Draw };

    // Unlike Java, we can't put actual fields and methods into enums themselves. So we still need these separate classes.
    interface IHand
    {
      RPSHands EnumValue { get; }

      /// <summary>
      /// How many points the hand is worth just by playing it.
      /// </summary>
      int ScoreHand { get; }

      /// <summary>
      /// Determines how this hand fares against an opponent's hand.
      /// </summary>
      /// <param name="opponent">Opponent hand to play against.</param>
      /// <returns>Outcome of the battle.</returns>
      RPSOutcome GetBattleOutcome(IHand opponent);

      /// <summary>
      /// Determines which hand needs to be thrown against this hand to reach the specified outcome.
      /// </summary>
      /// <param name="outcome">The desired outcome.</param>
      /// <returns>The hand to throw against this hand.</returns>
      IHand GetHandForDesiredOutcome(RPSOutcome outcome);
    }

    sealed class Rock : IHand
    {
      public static Rock Singleton { get; } = new Rock();

      private Rock()
      {
        // singleton only
      }

      public RPSHands EnumValue => RPSHands.Rock;
      public int ScoreHand => 1;

      public RPSOutcome GetBattleOutcome(IHand opponent)
      {
        return opponent.EnumValue switch
        {
          RPSHands.Rock => RPSOutcome.Draw,
          RPSHands.Paper => RPSOutcome.Loss,
          RPSHands.Scissors => RPSOutcome.Win,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }

      public IHand GetHandForDesiredOutcome(RPSOutcome outcome)
      {
        return outcome switch
        {
          RPSOutcome.Win => Paper.Singleton,
          RPSOutcome.Loss => Scissors.Singleton,
          RPSOutcome.Draw => Rock.Singleton,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }
    }

    sealed class Paper : IHand
    {
      public static Paper Singleton { get; } = new Paper();

      private Paper()
      {
        // singleton only
      }

      public RPSHands EnumValue => RPSHands.Paper;
      public int ScoreHand => 2;

      public RPSOutcome GetBattleOutcome(IHand opponent)
      {
        return opponent.EnumValue switch
        {
          RPSHands.Rock => RPSOutcome.Win,
          RPSHands.Paper => RPSOutcome.Draw,
          RPSHands.Scissors => RPSOutcome.Loss,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }

      public IHand GetHandForDesiredOutcome(RPSOutcome outcome)
      {
        return outcome switch
        {
          RPSOutcome.Win => Scissors.Singleton,
          RPSOutcome.Loss => Rock.Singleton,
          RPSOutcome.Draw => Paper.Singleton,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }
    }

    sealed class Scissors : IHand
    {
      public static Scissors Singleton { get; } = new Scissors();

      private Scissors()
      {
        // singleton only
      }

      public RPSHands EnumValue => RPSHands.Scissors;
      public int ScoreHand => 3;

      public RPSOutcome GetBattleOutcome(IHand opponent)
      {
        return opponent.EnumValue switch
        {
          RPSHands.Rock => RPSOutcome.Loss,
          RPSHands.Paper => RPSOutcome.Win,
          RPSHands.Scissors => RPSOutcome.Draw,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }

      public IHand GetHandForDesiredOutcome(RPSOutcome outcome)
      {
        return outcome switch
        {
          RPSOutcome.Win => Rock.Singleton,
          RPSOutcome.Loss => Paper.Singleton,
          RPSOutcome.Draw => Scissors.Singleton,
          _ => throw new NotImplementedException("Invalid enum value")
        };
      }
    }
  }
}
