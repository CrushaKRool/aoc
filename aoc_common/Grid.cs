namespace aoc_common
{
  public class Grid
  {
    public char[][] Data { get; }
    public int XMax { get; }
    public int YMax { get; }

    public Grid(string input)
    {
      string[] lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      Data = lines.Select(s => s.ToArray()).ToArray();
      XMax = lines[0].Length;
      YMax = lines.Length;
    }

    public bool PointInGrid(int x, int y)
    {
      return x >= 0 && x < XMax && y >= 0 && y < YMax;
    }

    public void Print()
    {
      for (int y = 0; y < YMax; y++)
      {
        for (int x = 0; x < XMax; x++)
        {
          Console.Write(Data[y][x]);
        }
        Console.WriteLine();
      }
    }
  }
}
