using System;
using AdventOfCode.Day7;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day7;

public class Day7Tests
{
    [Test]
    public void GetSumOfSizesOfDirectories_WithMaxSize100000AndSampleData_ReturnsExpectedResult()
    {
        var terminalOutput = ParseTerminalOutput();
        Day7Puzzle.GetSumOfSizesOfDirectories(terminalOutput, 100000).Should().Be(95437);
    }

    private static Command[] ParseTerminalOutput()
    {
        // TODO: 
        // var lines = File.ReadAllLines("Day7\\Part1.sample.txt");
        // ...

        return new Command[]
        {
            new RootDirectoryCommand(),
            new ListDirectoryContentsCommand(new[]
                {
                    new DirectorySummary("a"),
                },
                new[]
                {
                    new FileSummary("b.txt", 14848514),
                    new FileSummary("c.dat", 8504156),
                }),
            new RelativeChangeDirectoryCommand("a"),
            new ListDirectoryContentsCommand(Array.Empty<DirectorySummary>(), new []
            {
                new FileSummary("x.txt", 90000)
            })
        };
    }

    [Test]
    public void GetSumOfSizesOfDirectories_WithMaxSize100000AndRealData_ReturnsExpectedResult()
    {
    }
}