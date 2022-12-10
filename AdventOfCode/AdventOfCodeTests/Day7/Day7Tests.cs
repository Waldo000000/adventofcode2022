using System.IO;
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
        Day7Puzzle.GetSumOfSizesOfDirectories(terminalOutput).Should().Be(95437);
    }

    private static TerminalOutput ParseTerminalOutput()
    {
        // TODO: 
        // var lines = File.ReadAllLines("Day7\\Part1.sample.txt");
        // ...

        var terminalOutput = new TerminalOutput(
            new TerminalCommandOutput[]
            {
                new ChangeDirectoryTerminalCommandOutput("/"),
                new ListDirectoryContentsTerminalCommandOutput(new[]
                    {
                        new DirectorySummary("a"),
                        new DirectorySummary("a"),
                    },
                    new[]
                    {
                        new FileSummary("b.txt", 14848514),
                        new FileSummary("c.dat", 8504156),
                    })
            });
        return terminalOutput;
    }

    [Test]
    public void GetSumOfSizesOfDirectories_WithMaxSize100000AndRealData_ReturnsExpectedResult()
    {
    }
}
