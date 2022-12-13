using aoc_common;
using System;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day02
{
  public class Day02 : IPuzzle
  {
    public string PuzzleName => "Day 2: Rock Paper Scissors";

    public string InputFileName => @"Puzzles\Day02\Day02Input.txt";

    private const int ScoreLoss = 0;
    private const int ScoreDraw = 3;
    private const int ScoreWin = 6;

    private readonly Regex LineParser = new("([ABC]) ([XYZ])");

    public void Run(string input)
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
          switch (battleOutcome)
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
      Console.WriteLine($"Executing the given unclear strategy would result in a score of {score}.");

      // Part 2
      score = 0;
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
      Console.WriteLine($"Executing the given decrypted strategy would result in a score of {score}.");
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
  }
}