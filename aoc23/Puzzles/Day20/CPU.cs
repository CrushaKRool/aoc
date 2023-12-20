using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace aoc23.Puzzles.Day20
{
  internal class CPU
  {
    public Dictionary<string, Module> Modules { get; } = [];
    public Queue<Pulse> PulseQueue { get; } = new();
    public long HighCount { get; set; } = 0;
    public long LowCount { get; set; } = 0;

    public bool LowRxSent { get; set; } = false;

    public CPU(string input)
    {
      Dictionary<string, Conjunction> conjunctionModules = [];

      foreach (string inputLine in input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
        string[] parts = inputLine.Split(" -> ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string name = parts[0];
        string[] dest = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        Module module;
        if (name.StartsWith('%'))
        {
          module = new FlipFlop(name[1..], dest);
        }
        else if (name.StartsWith('&'))
        {
          module = new Conjunction(name[1..], dest);
          conjunctionModules.Add(module.Name, (Conjunction)module);
        }
        else if (name.Equals("broadcaster"))
        {
          module = new Broadcast(name, dest);
        }
        else
        {
          throw new ArgumentException($"Unknown module name: {name}");
        }
        Modules[module.Name] = module;
      }

      foreach (var module in Modules.Values)
      {
        foreach (var dest in module.Destinations)
        {
          if (conjunctionModules.TryGetValue(dest, out Conjunction? conjunction))
          {
            conjunction.RegisterInput(module.Name);
          }
        }
      }
    }

    public void PushButton()
    {
      SendPulse(false, "button", "broadcaster");

      while (PulseQueue.TryDequeue(out Pulse? pulse))
      {
        if (Modules.TryGetValue(pulse.Destination, out Module? destination))
        {
          destination.ReceivePulse(pulse, this);
        }
      }
    }

    public void SendPulse(bool high, string source, params string[] destinations)
    {
      foreach (string target in destinations)
      {
        PulseQueue.Enqueue(new(source, target, high));
        if (high)
        {
          HighCount++;
        }
        else
        {
          LowCount++;
          if (target.Equals("rx"))
          {
            LowRxSent = true;
          }
        }
      }
    }
  }
}
