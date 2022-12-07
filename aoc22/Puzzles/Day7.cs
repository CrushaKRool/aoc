using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles
{
  /// <summary>
  /// Not my best work... this is quite some spaghetti.
  /// </summary>
  class Day7 : IPuzzleSolver
  {
    private readonly Regex CommandParser = new("\\$ (\\S+)(.*?)$");
    private readonly Regex DirectoryParser = new("dir (\\S+)");
    private readonly Regex FileParser = new("(\\d+) (\\S+)");

    private readonly List<DirectoryNode> AllDirs = new();
    private readonly DirectoryNode Root = new("/", null);
    private DirectoryNode? CurrentDir;

    private bool IsLSParsing = false; // Quick and dirty hacky flag, until refactoring.

    public string PuzzleName => "Day 7: No Space Left On Device";

    public string SolvePart1(string input)
    {
      ParseDirectoryTree(input);

      long totalSize = AllDirs.Where(n => n.Size <= 100000).Sum(n => n.Size);

      return $"The sum of total directory sizes is {totalSize}.";
    }

    public string SolvePart2(string input)
    {
      ParseDirectoryTree(input);

      long totalSpace = 70_000_000;
      long requiredSpace = 30_000_000;
      long usedSpace = Root.Size;
      long unusedSpace = totalSpace - usedSpace;
      long spaceToFree = requiredSpace - unusedSpace;

      if (spaceToFree <= 0)
      {
        throw new InvalidOperationException("There is already enough free space. Puzzle input does not make any sense.");
      }

      // Go through all directories that have at least as much file size as we need to free.
      // Then sort them by ascending size. It follows that the first entry will the smallest one that satisfies our needs.
      DirectoryNode nodeToDelete = AllDirs.Where(n => n.Size >= spaceToFree).OrderBy(n => n.Size).First();

      return $"We should delete directory {nodeToDelete.Name}, which has a total size of {nodeToDelete.Size}.";
    }

    private void ParseDirectoryTree(string input)
    {
      AllDirs.Clear();
      Root.Children.Clear();
      CurrentDir = null;

      foreach (string line in input.Split(Environment.NewLine))
      {
        if (line.StartsWith("$"))
        {
          OnCommand(line);
        }
        else
        {
          OnParameterLine(line);
        }
      }
    }

    private void OnCommand(string line)
    {
      IsLSParsing = false;
      Match m = CommandParser.Match(line);
      if (!m.Success)
      {
        throw new ArgumentException("OnCommand called without a command input!");
      }

      switch(m.Groups[1].Value)
      {
        case "cd":
          CommandCD(m.Groups[2].Value.Trim());
          break;
        case "ls":
          CommandLS();
          break;
      }
    }

    private void CommandCD(string newDir)
    {
      if ("/".Equals(newDir))
      {
        CurrentDir = Root;
        return;
      }
      if ("..".Equals(newDir))
      {
        if (CurrentDir == null || CurrentDir.Parent == null)
        {
          CurrentDir = Root;
        }
        else
        {
          CurrentDir = CurrentDir.Parent;
        }
        return;
      }
      CurrentDir = CurrentDir.Children.OfType<DirectoryNode>().First(n => n.Name.Equals(newDir));
    }

    private void CommandLS()
    {
      IsLSParsing = true;
    }

    private void OnParameterLine(string line)
    {
      if (string.IsNullOrEmpty(line))
      {
        return;
      }
      if (!IsLSParsing)
      {
        throw new InvalidOperationException("Non-command line received while not in parsing state!");
      }

      Match m = DirectoryParser.Match(line);
      if (m.Success)
      {
        DirectoryNode newDirNode = new(m.Groups[1].Value, CurrentDir);
        AllDirs.Add(newDirNode);
        CurrentDir.Children.Add(newDirNode);
        return;
      }
      m = FileParser.Match(line);
      if (m.Success)
      {
        CurrentDir.Children.Add(new FileNode(m.Groups[2].Value, long.Parse(m.Groups[1].Value)));
      }
    }

    public abstract class Node
    {
      public string Name { get; }
      public virtual long Size { get; }

      protected Node(string name)
      {
        Name = name;
      }

      public override bool Equals(object? obj)
      {
        return obj is Node node &&
               Name == node.Name;
      }

      public override int GetHashCode()
      {
        return HashCode.Combine(Name);
      }

      public override string ToString()
      {
        return Name;
      }
    }

    public class DirectoryNode : Node
    {
      public DirectoryNode? Parent { get; }
      public ISet<Node> Children { get; set; } = new HashSet<Node>();

      public DirectoryNode(string name, DirectoryNode? parent) : base(name)
      {
        Parent = parent;
      }

      public override long Size => Children.Sum(n => n.Size);
    }

    public class FileNode : Node
    {
      private readonly long _size;

      public FileNode(string name, long size) : base(name)
      {
        _size = size;
      }

      public override long Size => _size;
    }
  }
}
