namespace aoc23.Puzzles.Day11
{
  internal class Universe
  {
    public List<Galaxy> Galaxies { get; } = [];
    private long xMax;
    private long yMax;

    public Universe(string input)
    {
      int y = 0;
      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        for (int x = 0; x < inputLine.Length; x++)
        {
          char c = inputLine[x];
          if (c == '#')
          {
            Galaxies.Add(new(x, y));
          }
        }
        xMax = Math.Max(xMax, inputLine.Length - 1);
        y++;
      }
      yMax = y - 1;
    }

    public void Expand(long factor)
    {
      // We need to "replace" one existing column, so the result should be 1 less than the factor.
      factor--;

      int xExpansions = 0;
      int yExpansions = 0;
      for (long x = xMax; x >= 0; x--)
      {
        if (!Galaxies.Exists(g => g.X == x))
        {
          xExpansions++;
          foreach (Galaxy g in Galaxies.Where(g => g.X > x))
          {
            g.X += factor;
          }
        }
      }
      xMax += xExpansions * factor;

      for (long y = yMax; y >= 0; y--)
      {
        if (!Galaxies.Exists(g => g.Y == y))
        {
          yExpansions++;
          foreach (Galaxy g in Galaxies.Where(g => g.Y > y))
          {
            g.Y += factor;
          }
        }
      }
      yMax += yExpansions * factor;
    }

    public IEnumerable<Tuple<Galaxy, Galaxy>> GetGalaxyPairs()
    {
      var galaxyQueue = new Queue<Galaxy>(Galaxies);
      while (galaxyQueue.TryDequeue(out Galaxy? curGal))
      {
        foreach (var otherGal in galaxyQueue)
        {
          yield return new(curGal, otherGal);
        }
      }
    }

    public void Visualize()
    {
      for (long y = 0; y <= yMax; y++)
      {
        for (long x = 0; x <= xMax; x++)
        {
          if (Galaxies.Exists(g => g.X == x && g.Y == y))
          {
            Console.Write('#');
          }
          else if (!Galaxies.Exists(g => g.X == x) || !Galaxies.Exists(g => g.Y == y))
          {
            Console.Write('0');
          }
          else
          {
            Console.Write('.');
          }
        }
        Console.WriteLine();
      }
    }
  }
}
