namespace aoc23.Puzzles.Day09
{
  internal class History
  {
    int[] Values { get; }

    public History(string inputLine)
    {
      Values = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray();
    }

    public int PredictNextValue()
    {
      List<int> extrapolated = ExtrapolateNext([.. Values]);
      return extrapolated[^1];
    }

    public int PredictPreviousValue()
    {
      List<int> extrapolated = ExtrapolatePrevious([.. Values]);
      return extrapolated[0];
    }

    private List<int> ExtrapolateNext(List<int> list)
    {
      if (list.TrueForAll(i => i == 0))
      {
        list.Add(0);
      }
      else
      {
        List<int> derivative = GetDerivative(list);
        ExtrapolateNext(derivative);
        list.Add(list[^1] + derivative[^1]);
      }
      return list;
    }

    private List<int> ExtrapolatePrevious(List<int> list)
    {
      if (list.TrueForAll(i => i == 0))
      {
        list.Insert(0, 0);
      }
      else
      {
        List<int> derivative = GetDerivative(list);
        ExtrapolatePrevious(derivative);
        list.Insert(0, list[0] - derivative[0]);
      }
      return list;
    }

    private List<int> GetDerivative(List<int> list)
    {
      List<int> derivative = [];
      for (int i = 0; i < list.Count - 1; i++)
      {
        derivative.Add(list[i + 1] - list[i]);
      }
      return derivative;
    }
  }
}
