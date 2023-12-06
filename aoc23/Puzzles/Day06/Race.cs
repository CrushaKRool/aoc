namespace aoc23.Puzzles.Day06
{
  internal record Race(long Time, long Distance)
  {
    public int GetNumRecordBeatingPermutations()
    {
      int recordsBeaten = 0;
      for (long i = 1; i < Time; i++)
      {
        long achievedDistance = i * (Time - i);
        if (achievedDistance > Distance)
        {
          recordsBeaten++;
        }
      }
      return recordsBeaten;
    }
  }
}
