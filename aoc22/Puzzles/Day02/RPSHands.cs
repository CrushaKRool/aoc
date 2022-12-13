using System;

namespace aoc22.Puzzles.Day02
{
  internal interface IHand
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

  internal sealed class Rock : IHand
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
        RPSOutcome.Draw => Singleton,
        _ => throw new NotImplementedException("Invalid enum value")
      };
    }
  }

  internal sealed class Paper : IHand
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
        RPSOutcome.Draw => Singleton,
        _ => throw new NotImplementedException("Invalid enum value")
      };
    }
  }

  internal sealed class Scissors : IHand
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
        RPSOutcome.Draw => Singleton,
        _ => throw new NotImplementedException("Invalid enum value")
      };
    }
  }
}