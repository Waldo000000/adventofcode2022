using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day9;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day9;

public class Day9Tests
{
    [Test]
    public void GetNumberOfUniqueTailPositions_WithSampleData_ReturnsExpectedResult()
    {
        var motions = ReadTreeMap("Day9\\Part1.sample.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions).Should().Be(13);
    }

    [Test]
    public void GetNumberOfUniqueTailPositions_WithRealData_ReturnsExpectedResult()
    {
        var motions = ReadTreeMap("Day9\\Part1.real.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions).Should().Be(6311);
    }

    private static Motion[] ReadTreeMap(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        return lines.Select(ParseMotion).ToArray();
    }

    private static Motion ParseMotion(string line)
    {
        var tokens = line.Split(" ");
        return new Motion(ParseDirection(tokens[0]), int.Parse(tokens[1]));
    }

    private static Direction ParseDirection(string token) => token switch
    {
        "U" => Direction.Up,
        "D" => Direction.Down,
        "L" => Direction.Left,
        "R" => Direction.Right,
        _ => throw new ArgumentOutOfRangeException($"Unrecognized direction token {token}")
    };
}