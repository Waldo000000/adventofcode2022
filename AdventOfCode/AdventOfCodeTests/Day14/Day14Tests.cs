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
        var rockPaths = ReadRockPaths("Day13\\Part1.sample.txt").ToArray();

        var sandOrigin = new Coord(500,0);
        
        Day14Puzzle.GetFinalCountOfRestingSand(sandOrigin, rockPaths).Should().Be(24);
    }

    [Test]
    public void GetSumOfIndicesOfOrderedPairs_WithRealData_ReturnsExpectedValue()
    {
        var rockPaths = ReadRockPaths("Day13\\Part1.real.txt").ToArray();

        var sandOrigin = new Coord(500,0);

        Day14Puzzle.GetFinalCountOfRestingSand(sandOrigin, rockPaths).Should().Be(24);
    }

    private RockPath[] ReadRockPaths(string filename)
    {
        return new[]
        {
            new RockPath(new Coord(498, 4), new Coord(498, 6), new Coord(496, 6)),
            new RockPath(new Coord(503, 4), new Coord(502, 4), new Coord(502, 9), new Coord(494, 9))
        };

        // TODO: string parsing
    }
}