using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc22.Puzzles.Day23
{
  internal class Elf
  {
    /// <summary>
    /// Ordered list of MoveProposals that the elves may want to perform.
    /// </summary>
    private static readonly MoveProposal[] Proposals = new MoveProposal[]
    {
      new(CardinalDirection.N, CardinalDirection.N, CardinalDirection.NE, CardinalDirection.NW),
      new(CardinalDirection.S, CardinalDirection.S, CardinalDirection.SE, CardinalDirection.SW),
      new(CardinalDirection.W, CardinalDirection.W, CardinalDirection.NW, CardinalDirection.SW),
      new(CardinalDirection.E, CardinalDirection.E, CardinalDirection.NE, CardinalDirection.SE),
    };

    public Point Location { get; set; }
    public Point ProposedLocation { get; set; }
    public CardinalDirection? ProposedMove { get; set; }

    public void ProposeLocation(IEnumerable<Elf> allElves, int round)
    {
      ProposedLocation = Location; // If we can't move anywhere, stay where we are.
      ProposedMove = null;

      if (!OtherElvesInRange(allElves, 1))
      {
        // No need to move if we found a good spot.
        return;
      }

      for (int i = 0; i < Proposals.Length; i++)
      {
        int index = (round + i) % Proposals.Length;
        MoveProposal proposal = Proposals[index];
        if (proposal.CanMove(this, allElves))
        {
          ProposedLocation = new Point(Location.X + proposal.MoveDirection.MoveOffsetX, Location.Y + proposal.MoveDirection.MoveOffsetY);
          ProposedMove = proposal.MoveDirection;
          break;
        }
      }
    }

    private bool OtherElvesInRange(IEnumerable<Elf> allElves, int range)
    {
      return allElves.Where(elf => elf != this).Any(other => Math.Abs(other.Location.X - Location.X) <= range && Math.Abs(other.Location.Y - Location.Y) <= range);
    }

    public override string ToString()
    {
      return $"Elf: {Location}; ({ProposedLocation})";
    }
  }
}
