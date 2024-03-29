﻿using aoc_common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc22.Puzzles.Day07
{
  /// <summary>
  /// Not my best work... this is quite some spaghetti.
  /// </summary>
  public class Day07 : IPuzzle
  {
    private readonly Regex CommandParser = new("\\$ (\\S+)(.*?)$");
    private readonly Regex DirectoryParser = new("dir (\\S+)");
    private readonly Regex FileParser = new("(\\d+) (\\S+)");

    private readonly List<DirectoryNode> AllDirs = new();
    private readonly DirectoryNode Root = new("/", null);
    private DirectoryNode? CurrentDir;

    private bool IsLSParsing = false; // Quick and dirty hacky flag, until refactoring.

    public string PuzzleName => "Day 7: No Space Left On Device";

    public string InputFileName => @"Input.txt";

    public void Run(string input)
    {
      ParseDirectoryTree(input);

      long totalSize = AllDirs.Where(n => n.Size <= 100000).Sum(n => n.Size);

      Console.WriteLine($"The sum of total directory sizes is {totalSize}.");

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

      Console.WriteLine($"We should delete directory {nodeToDelete.Name}, which has a total size of {nodeToDelete.Size}.");
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

      switch (m.Groups[1].Value)
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
  }
}