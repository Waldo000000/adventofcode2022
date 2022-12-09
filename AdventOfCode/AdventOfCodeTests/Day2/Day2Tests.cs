using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day2;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day2;

public class Day2Tests
{
    [Test]
    public void GetTotalScore_WithSampleData_ReturnsExpectedResult()
    {
        var strategyGuide = ReadStrategyGuide("Day2\\Part1.sample.txt");
        Day2Puzzle.GetTotalScore(strategyGuide).Should().Be(15);
    }
    
    private static StrategyGuide ReadStrategyGuide(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        return Day2Puzzle.DecryptStrategyGuide(lines);
    }
}
