﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day7;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day7;

public class Day7Tests
{
    private static readonly Regex DirectorySummaryRegex = new Regex(@"^dir ([\w]*)$");
    private static readonly Regex FileSummaryRegex = new Regex(@"^([\d]+) (.*)$");
    private static readonly Regex CommandWithOutputRegex = new Regex(@"\$[^\$]*");
    private static readonly Regex RootDirectoryCommandRegex = new Regex(@"\$ cd /");
    private static readonly Regex UpDirectoryCommandRegex = new Regex(@"\$ cd \.\.");
    private static readonly Regex RelativeChangeDirectoryCommandRegex = new Regex(@"\$ cd ([\w]+)");
    private static readonly Regex ListDirectoryContentsCommandRegex = new Regex(@"\$ ls", RegexOptions.Multiline);

    [Test]
    public void GetSumOfSizesOfDirectories_WithMaxSize100000AndSampleData_ReturnsExpectedResult()
    {
        var terminalOutput = ParseTerminalOutput("Day7\\Part1.sample.txt").ToArray();
        Day7Puzzle.GetSumOfSizesOfDirectories(terminalOutput, 100000).Should().Be(95437);
    }
    
    [Test]
    public void GetSumOfSizesOfDirectories_WithMaxSize100000AndRealData_ReturnsExpectedResult()
    {
        var terminalOutput = ParseTerminalOutput("Day7\\Part1.real.txt").ToArray();
        Day7Puzzle.GetSumOfSizesOfDirectories(terminalOutput, 100000).Should().Be(1086293);
    }
    
    [Test]
    public void GetSizeOfSmallestDirectoryBigEnoughToFreeSpace_WithMaxUsedAfterDelete40000000AndSampleData_ReturnsExpectedResult()
    {
        var terminalOutput = ParseTerminalOutput("Day7\\Part1.sample.txt").ToArray();
        Day7Puzzle.GetSizeOfSmallestDirectoryBigEnoughToFreeSpace(terminalOutput, 40000000).Should().Be(24933642);
    }
    
    [Test]
    public void GetSizeOfSmallestDirectoryBigEnoughToFreeSpace_WithMaxUsedAfterDelete40000000AndRealData_ReturnsExpectedResult()
    {
        var terminalOutput = ParseTerminalOutput("Day7\\Part1.real.txt").ToArray();
        Day7Puzzle.GetSizeOfSmallestDirectoryBigEnoughToFreeSpace(terminalOutput, 40000000).Should().Be(366028);
    }

    private static IEnumerable<Command> ParseTerminalOutput(string filename)
    {
        var text = System.IO.File.ReadAllText(filename);

        return CommandWithOutputRegex.Matches(text).Select(ParseCommand);
    }

    private static Command ParseCommand(Match commandWithOutput)
    {
        if (RootDirectoryCommandRegex.IsMatch(commandWithOutput.Value))
            return new RootDirectoryCommand();
        if (UpDirectoryCommandRegex.IsMatch(commandWithOutput.Value))
            return new UpDirectoryCommand();
        if (RelativeChangeDirectoryCommandRegex.IsMatch(commandWithOutput.Value))
            return new RelativeChangeDirectoryCommand(RelativeChangeDirectoryCommandRegex
                .Match(commandWithOutput.Value).Groups[1].ToString());
        if (ListDirectoryContentsCommandRegex.IsMatch(commandWithOutput.Value))
        {
            var summaries = commandWithOutput.Value
                .Split(Environment.NewLine)
                .Skip(1)
                .ToList();

            var directories = summaries
                .Where(summary => DirectorySummaryRegex.IsMatch(summary))
                .Select(summary => new DirectorySummary(DirectorySummaryRegex.Match(summary).Groups[1].ToString()))
                .ToArray();

            var files = summaries
                .Where(summary => FileSummaryRegex.IsMatch(summary))
                .Select(summary =>
                {
                    var match = FileSummaryRegex.Match(summary);
                    return new FileSummary(
                        match.Groups[2].ToString(),
                        Convert.ToInt32(match.Groups[1].Value)
                    );
                })
                .ToArray();

            return new ListDirectoryContentsCommand(directories, files);
        }

        throw new Exception($"Couldn't parse command: {commandWithOutput}");
    }
}