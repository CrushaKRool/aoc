namespace aoc23.Puzzles.Day20
{
  internal abstract class Module(string name, string[] destinations)
  {
    public string Name { get; set; } = name;
    public string[] Destinations { get; } = destinations;

    public abstract void ReceivePulse(Pulse pulse, CPU cpu);
  }

  internal class FlipFlop(string name, string[] destinations) : Module(name, destinations)
  {
    private bool On { get; set; } = false;

    public override void ReceivePulse(Pulse pulse, CPU cpu)
    {
      if (pulse.Low)
      {
        On = !On;
        cpu.SendPulse(On, Name, Destinations);
      }
    }
  }

  internal class Conjunction(string name, string[] destinations) : Module(name, destinations)
  {
    private Dictionary<string, bool> Inputs { get; } = [];

    public void RegisterInput(string name)
    {
      Inputs[name] = false;
    }

    public override void ReceivePulse(Pulse pulse, CPU cpu)
    {
      Inputs[pulse.Source] = pulse.High;
      cpu.SendPulse(!Inputs.Values.All(b => b), Name, Destinations);
    }
  }

  internal class Broadcast(string name, string[] destinations) : Module(name, destinations)
  {
    public override void ReceivePulse(Pulse pulse, CPU cpu)
    {
      cpu.SendPulse(pulse.High, Name, Destinations);
    }
  }
}
