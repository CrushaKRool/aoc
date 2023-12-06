namespace aoc23.Puzzles.Day05
{
  internal class Mapping(string name)
  {
    public string Name { get; } = name;
    public List<MappingRange> Ranges { get; } = [];

    public long MapValue(long value)
    {
      foreach (MappingRange r in Ranges)
      {
        if (r.ValueWithinRange(value))
        {
          return r.GetDestinationValue(value);
        }
      }
      return value;
    }
  }
}
