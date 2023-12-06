namespace aoc23.Puzzles.Day05
{
  internal class MappingRange
  {
    public long SourceStart { get; }
    public long DestinationStart { get; }
    public long RangeLength { get; }

    public MappingRange(string inputLine)
    {
      long[] inputNumbers = inputLine.Split(' ', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
      SourceStart = inputNumbers[1];
      DestinationStart = inputNumbers[0];
      RangeLength = inputNumbers[2];
    }

    public bool ValueWithinRange(long value)
    {
      return value >= SourceStart && value <= SourceStart + RangeLength;
    }

    /// <summary>
    /// This assumes that it has been previously checked that the value is within this range. Otherwise the results will be wrong.
    /// </summary>
    public long GetDestinationValue(long value)
    {
      return DestinationStart + (value - SourceStart);
    }
  }
}
