using System.IO;
using System.Linq;
using AdventOfCode.Day14;
using AdventOfCode.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day14;

public class Day14Tests
{
    [Test]
    public void GetFinalCountOfRestingSand_WithSampleData_ReturnsExpectedValue()
    {
        var rockPaths = ReadRockPaths("Day14\\Part1.sample.txt").ToArray();

        var sandOrigin = new Coord(500, 0);

        Day14Puzzle.GetFinalCountOfRestingSand(sandOrigin, rockPaths).Should().Be(24);
    }

    [Test]
    public void GetFinalCountOfRestingSand_WithRealData_ReturnsExpectedValue()
    {
        var rockPaths = ReadRockPaths("Day14\\Part1.real.txt").ToArray();

        var sandOrigin = new Coord(500, 0);

        Day14Puzzle.GetFinalCountOfRestingSand(sandOrigin, rockPaths).Should().Be(665);
    }

    [Test]
    public void GetFinalCountOfRestingSand_WithBedrockAndWithSampleData_ReturnsExpectedValue()
    {
        var rockPaths = ReadRockPaths("Day14\\Part1.sample.txt").ToArray();

        var sandOrigin = new Coord(500, 0);

        Day14Puzzle.GetFinalCountOfRestingSandWithBedrockOnceOriginBlocked(sandOrigin, rockPaths).Should().Be(93);
    }

    [Test]
    public void GetFinalCountOfRestingSand_WithBedrockAndWithRealData_ReturnsExpectedValue()
    {
        var rockPaths = ReadRockPaths("Day14\\Part1.real.txt").ToArray();

        var sandOrigin = new Coord(500, 0);

        Day14Puzzle.GetFinalCountOfRestingSandWithBedrockOnceOriginBlocked(sandOrigin, rockPaths).Should().Be(25434);
    }

    private RockPath[] ReadRockPaths(string filename)
    {
        return File.ReadAllLines(filename)
            .Select(ReadRockPath)
            .ToArray();
    }

    private RockPath ReadRockPath(string line)
    {
        var points = line
            .Split(" -> ")
            .Select(ReadCoord)
            .ToArray();

        return new RockPath(points);
    }

    private Coord ReadCoord(string str)
    {
        var tokens = str.Split(",");
        return new Coord(
            int.Parse(tokens[0]),
            int.Parse(tokens[1])
        );
    }
}