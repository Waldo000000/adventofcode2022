using System.IO;
using AdventOfCode.Day6;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day6;

public class Day6Tests
{
    [Test]
    public void GetCharactersProcessedBeforeStartOfPacketMarker_WithSampleData_ReturnsExpectedResult()
    {
        using StreamReader sr = new StreamReader("Day6\\Part1.sample.txt");
        Day6Puzzle.GetCharactersProcessedBeforeStartOfPacketMarker(sr).Should().Be((7, true));
    }
    
    [Test]
    public void GetCharactersProcessedBeforeStartOfPacketMarker_WithRealData_ReturnsExpectedResult()
    {
        using StreamReader sr = new StreamReader("Day6\\Part1.real.txt");
        Day6Puzzle.GetCharactersProcessedBeforeStartOfPacketMarker(sr).Should().Be((1987, true));
    }
    
    [Test]
    public void GetCharactersProcessedBeforeStartOfMessageMarker_WithSampleData_ReturnsExpectedResult()
    {
        using StreamReader sr = new StreamReader("Day6\\Part1.sample.txt");
        Day6Puzzle.GetCharactersProcessedBeforeStartOfMessageMarker(sr).Should().Be((19, true));
    }
    
    [Test]
    public void GetCharactersProcessedBeforeStartOfMessageMarker_WithRealData_ReturnsExpectedResult()
    {
        using StreamReader sr = new StreamReader("Day6\\Part1.real.txt");
        Day6Puzzle.GetCharactersProcessedBeforeStartOfMessageMarker(sr).Should().Be((3059, true));
    }
}
