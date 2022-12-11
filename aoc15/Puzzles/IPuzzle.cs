using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aoc15.Puzzles
{
  internal interface IPuzzle
  {
    void Run(string input);

    string GetInputFileName();
  }
}
