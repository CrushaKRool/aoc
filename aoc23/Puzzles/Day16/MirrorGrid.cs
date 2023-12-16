using aoc_common;
using System.Drawing;

namespace aoc23.Puzzles.Day16
{
  internal class MirrorGrid : Grid
    {
    public List<LightWalker> Walkers { get; } = [];
    public bool[,] Energized { get; set; }
    private readonly HashSet<Point> VisitedSplitters = [];

    public MirrorGrid(string input) : base(input)
    {
      Energized = new bool[XMax, YMax];
    }

    public int GetEnergizedCountForStartConfiguration(int startX, int startY, EDirection startDirection)
    {
      Energized = new bool[XMax, YMax];
      VisitedSplitters.Clear();
      Walkers.Clear();
      Walkers.Add(new(startX, startY, startDirection));

      int movesSinceEnergizedChanged = 0;
      do
      {
        bool energizedChanged = MoveAll();
        if (energizedChanged)
        {
          movesSinceEnergizedChanged = 0;
        }
        else
        {
          movesSinceEnergizedChanged++;
        }
      }
      while (movesSinceEnergizedChanged < 100);

      return GetEnergizedCount();
    }

    public bool MoveAll()
    {
      bool energizedChanged = false;
      for (int i = 0; i < Walkers.Count; i++)
      {
        LightWalker walker = Walkers[i];
        // Move according to direction.
        switch (walker.Direction)
        {
          case EDirection.UP:
            walker.Y--;
            break;
          case EDirection.DOWN:
            walker.Y++;
            break;
          case EDirection.LEFT:
            walker.X--;
            break;
          case EDirection.RIGHT:
            walker.X++;
            break;
        }

        // Remove this walker if outside the grid.
        if (!PointInGrid(walker.X, walker.Y) || WalkerIsAtVisitedSplitter(walker))
        {
          Walkers.Remove(walker);
          i--;
          continue;
        }

        // Energize and remember if not already energized.
        if (!Energized[walker.X, walker.Y])
        {
          energizedChanged = true;
          Energized[walker.X, walker.Y] = true;
        }

        char curTile = Data[walker.Y][walker.X];
        switch (walker.Direction)
        {
          case EDirection.UP:
            if (curTile == '/')
            {
              walker.Direction = EDirection.RIGHT;
            }
            else if (curTile == '\\')
            {
              walker.Direction = EDirection.LEFT;
            }
            else if (curTile == '-')
            {
              walker.Direction = EDirection.LEFT;
              Walkers.Add(new(walker.X, walker.Y, EDirection.RIGHT));
              VisitedSplitters.Add(new(walker.X, walker.Y));
            }
            break;
          case EDirection.DOWN:
            if (curTile == '/')
            {
              walker.Direction = EDirection.LEFT;
            }
            else if (curTile == '\\')
            {
              walker.Direction = EDirection.RIGHT;
            }
            else if (curTile == '-')
            {
              walker.Direction = EDirection.LEFT;
              Walkers.Add(new(walker.X, walker.Y, EDirection.RIGHT));
              VisitedSplitters.Add(new(walker.X, walker.Y));
            }
            break;
          case EDirection.LEFT:
            if (curTile == '/')
            {
              walker.Direction = EDirection.DOWN;
            }
            else if (curTile == '\\')
            {
              walker.Direction = EDirection.UP;
            }
            else if (curTile == '|')
            {
              walker.Direction = EDirection.UP;
              Walkers.Add(new(walker.X, walker.Y, EDirection.DOWN));
              VisitedSplitters.Add(new(walker.X, walker.Y));
            }
            break;
          case EDirection.RIGHT:
            if (curTile == '/')
            {
              walker.Direction = EDirection.UP;
            }
            else if (curTile == '\\')
            {
              walker.Direction = EDirection.DOWN;
            }
            else if (curTile == '|')
            {
              walker.Direction = EDirection.UP;
              Walkers.Add(new(walker.X, walker.Y, EDirection.DOWN));
              VisitedSplitters.Add(new(walker.X, walker.Y));
            }
            break;
        }
      }
      return energizedChanged;
    }

    public bool WalkerIsAtVisitedSplitter(LightWalker walker)
    {
      char curTile = Data[walker.Y][walker.X];
      if ((curTile == '-' && (walker.Direction == EDirection.UP || walker.Direction == EDirection.DOWN))
        || (curTile == '|' && (walker.Direction == EDirection.LEFT || walker.Direction == EDirection.RIGHT)))
      {
        return VisitedSplitters.Contains(new Point(walker.X, walker.Y));
      }
      return false;
    }

    public int GetEnergizedCount()
    {
      int sum = 0;
      for (int y = 0; y < YMax; y++)
      {
        for (int x = 0; x < XMax; x++)
        {
          if (Energized[x,y])
          {
            sum++;
          }
        }
      }
      return sum;
    }

    public void PrintEnergized()
    {
      for (int y = 0; y < YMax; y++)
      {
        for (int x = 0; x < XMax; x++)
        {
          if (Walkers.Exists(w => w.X == x && w.Y == y))
          {
            Console.ForegroundColor = ConsoleColor.Red;
          }
          else if (Energized[x, y])
          {
            Console.ForegroundColor = ConsoleColor.Green;
          }
          Console.Write(Data[y][x]);
          Console.ResetColor();
        }
        Console.WriteLine();
      }
    }
  }
}
