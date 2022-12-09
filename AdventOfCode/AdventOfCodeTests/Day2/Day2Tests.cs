using System.Collections.Generic;
using System.IO;
using AdventOfCode.Day2;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day2;

public class Day2Tests
{
    [Test]
    public void GetTotalScore_WithSampleDataDecryptedFromHandShapeGuide_ReturnsExpectedResult()
    {
        var strategyGuide = ReadStrategyGuideFromHandShapeGuide("Day2\\Part1.sample.txt");
        Day2Puzzle.GetTotalScore(strategyGuide).Should().Be(15);
    }
    
    [Test]
    public void GetTotalScore_WithRealDataDecryptedFromHandShapeGuide_ReturnsExpectedResult()
    {
        var strategyGuide = ReadStrategyGuideFromHandShapeGuide("Day2\\Part1.real.txt");
        Day2Puzzle.GetTotalScore(strategyGuide).Should().Be(10994);
    }
    
    [Test]
    public void GetTotalScore_WithSampleDataDecryptedFromOutcomeGuide_ReturnsExpectedResult()
    {
        var strategyGuide = ReadStrategyGuideFromOutcomeGuide("Day2\\Part1.sample.txt");
        Day2Puzzle.GetTotalScore(strategyGuide).Should().Be(12);
    }
    
    [Test]
    public void GetTotalScore_WithRealDataDecryptedFromOutcomeGuide_ReturnsExpectedResult()
    {
        var strategyGuide = ReadStrategyGuideFromOutcomeGuide("Day2\\Part1.real.txt");
        Day2Puzzle.GetTotalScore(strategyGuide).Should().Be(12526);
    }
    
    private static StrategyGuide ReadStrategyGuideFromHandShapeGuide(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        return Day2Puzzle.DecryptStrategyGuideFromHandShapeGuide(lines);
    }
    
    private static StrategyGuide ReadStrategyGuideFromOutcomeGuide(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        return Day2Puzzle.DecryptStrategyGuideFromOutcomeGuide(lines);
    }
}
