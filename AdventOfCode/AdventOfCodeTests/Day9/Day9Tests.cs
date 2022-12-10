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
    public void GetNumberOfUniqueTailPositions_WithTwoKnotsWithSampleData_ReturnsExpectedResult()
    {
        var motions = ReadMotions("Day9\\Part1.sample.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions, new Rope()).Should().Be(13);
    }

    [Test]
    public void GetNumberOfUniqueTailPositions_WithTwoKnotsWithRealData_ReturnsExpectedResult()
    {
        var motions = ReadMotions("Day9\\Part1.real.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions, new Rope()).Should().Be(6311);
    }

    [Test]
    public void GetNumberOfUniqueTailPositions_WithTenKnotsWithPart1SampleData_ReturnsExpectedResult()
    {
        var motions = ReadMotions("Day9\\Part1.sample.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions, new Rope()).Should().Be(1);
    }
    
    [Test]
    public void GetNumberOfUniqueTailPositions_WithTenKnotsWithPart2SampleData_ReturnsExpectedResult()
    {
        var motions = ReadMotions("Day9\\Part2.sample.txt");
        Day9Puzzle.GetNumberOfUniqueTailPositions(motions, new Rope()).Should().Be(36);
    }

    private static Motion[] ReadMotions(string inputFilepath)
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