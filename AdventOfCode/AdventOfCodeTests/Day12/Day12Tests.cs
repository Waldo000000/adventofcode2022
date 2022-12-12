using AdventOfCode.Day12;
using AdventOfCode.Day9;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day11;

public class Day12Tests
{
    [Test]
    public void GetFewestStepsToBestSignal_WithSampleData_ReturnsExpectedValue()
    {
        var (currentPosition, bestSignalPosition, heightmap) = ReadHeightmap("Day12\\Part1.sample.txt");

        Day12Puzzle.GetFewestStepsToBestSignal(currentPosition, bestSignalPosition, heightmap).Should().Be(10605);
    }

    [Test] public void GetFewestStepsToBestSignal_WithRealData_ReturnsExpectedValue()
    {
        var (currentPosition, bestSignalPosition, heightmap) = ReadHeightmap("Day12\\Part1.sample.txt");

        Day12Puzzle.GetFewestStepsToBestSignal(currentPosition, bestSignalPosition, heightmap).Should().Be(-1); // TODO
    }

    private (Coord, Coord, Heightmap) ReadHeightmap(string filename)
    {
        Coord currentPosition = new Coord(0, 0);
        Coord bestSignalPosition = new Coord(0, 0);
        return (currentPosition, bestSignalPosition, new Heightmap(new int[,]{}));
        
        // TODO
        // return File.ReadAllLines(filename);
    }
}