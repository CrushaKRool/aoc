using aoc22.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aoc22
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void btnSolve_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Make the puzzle selectable.
      IPuzzleSolver solver = new Day11();
      try
      {
        SetOutput1(solver.SolvePart1(GetInput()));
      }
      catch (Exception ex)
      {
        SetOutput1($"ERROR: {ex.Message}");
      }
      try
      {
        SetOutput2(solver.SolvePart2(GetInput()));
      }
      catch (Exception ex)
      {
        SetOutput2($"ERROR: {ex.Message}");
      }
    }

    /// <summary>
    /// Gets the input text for the puzzle.
    /// </summary>
    /// <returns>The input for the puzzle.</returns>
    private string GetInput()
    {
      return tbInput.Text;
    }

    /// <summary>
    /// Shows the computed output of the puzzle in the UI.
    /// </summary>
    /// <param name="output">Output to show.</param>
    private void SetOutput1(string output)
    {
      tbOutput.Text = output;
    }

    /// <summary>
    /// Shows the computed output of the puzzle in the UI.
    /// </summary>
    /// <param name="output">Output to show.</param>
    private void SetOutput2(string output)
    {
      tbOutput2.Text = output;
    }
  }
}
