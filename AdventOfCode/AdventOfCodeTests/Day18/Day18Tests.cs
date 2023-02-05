using System;
using System.IO;
using System.Linq;
using AdventOfCode.Day18;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day18;

public class Day18Tests
{
    [Test]
    public void GetSurfaceArea_WithDummyData_ReturnsExpectedValue()
    {
        var lavaCubes = ReadLavaCubes("Day18\\Part1.dummy.txt").ToArray();

        Day18Puzzle.GetSurfaceArea(lavaCubes).Should().Be(10);
    }

    [Test]
    public void GetSurfaceArea_WithSampleData_ReturnsExpectedValue()
    {
        var lavaCubes = ReadLavaCubes("Day18\\Part1.sample.txt").ToArray();

        Day18Puzzle.GetSurfaceArea(lavaCubes).Should().Be(64);
    }

    [Test]
    public void GetSurfaceArea_WithRealData_ReturnsExpectedValue()
    {
        var lavaCubes = ReadLavaCubes("Day18\\Part1.real.txt").ToArray();

        Day18Puzzle.GetSurfaceArea(lavaCubes).Should().Be(3498);
    }

    private LavaCube[] ReadLavaCubes(string filename)
    {
        return File.ReadAllLines(filename).Select(line =>
        {
            var tokens = line.Split(",");
            return new LavaCube(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2])
            );
        }).ToArray();
    }
}