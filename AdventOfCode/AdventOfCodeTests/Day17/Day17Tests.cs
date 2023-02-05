using System;
using System.IO;
using System.Linq;
using AdventOfCode.Day17;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day17;

public class Day17Tests
{
    [Test]
    public void GetTowerHeightAfterNumRocksFallen_WithSampleData_ReturnsExpectedValue()
    {
        var jetPushPattern = ReadJetPattern("Day17\\Part1.sample.txt");

        Day17Puzzle.GetTowerHeightAfterNumRocksFallen(2022, jetPushPattern).Should().Be(3068);
    }
    
    [Test]
    public void GetTowerHeightAfterNumRocksFallen_WithRealData_ReturnsExpectedValue()
    {
        var jetPushPattern = ReadJetPattern("Day17\\Part1.real.txt");

        Day17Puzzle.GetTowerHeightAfterNumRocksFallen(2022, jetPushPattern).Should().Be(3232);
    }
    
    [Test]
    public void GetTowerHeightAfterNumRocksFallen_WithLargeTowerWithSampleData_ReturnsExpectedValue()
    {
        var jetPushPattern = ReadJetPattern("Day17\\Part1.sample.txt");

        Day17Puzzle.GetTowerHeightAfterNumRocksFallen(1000000000000L, jetPushPattern).Should().Be(1514285714288L);
    }
    
    [Test]
    public void GetTowerHeightAfterNumRocksFallen_WithLargeTowerWithRealData_ReturnsExpectedValue()
    {
        var jetPushPattern = ReadJetPattern("Day17\\Part1.real.txt");

        Day17Puzzle.GetTowerHeightAfterNumRocksFallen(1000000000000L, jetPushPattern).Should().Be(-1);
    }

    private JetPushPattern ReadJetPattern(string filename)
    {
        var jetPushes = File.ReadLines(filename).Single().ToCharArray().Select(ReadJetPush).ToArray();
        return new JetPushPattern(jetPushes);
    }

    private JetPush ReadJetPush(char ch)
    {
        return ch switch
        {
            '>' => JetPush.Right,
            '<' => JetPush.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}