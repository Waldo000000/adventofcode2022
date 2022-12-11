using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day10;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day10;

public class Day10Tests
{
    [Test]
    public void GetStates_WithDemoData_ReturnsExpectedValues()
    {
        var operations = ReadOperations("Day10\\Part1.demo.txt");
        var registerXValuesDuringCycles = Day10Puzzle
            .GetState(operations)
            .Select(state => state.RegisterXValue)
            .ToList();

        registerXValuesDuringCycles.Should().Equal(new[] {1, 1, 1, 4, 4});
    }

    [Test]
    public void GetSumOfSignalStrengths_WithSampleData_ReturnsExpectedResult()
    {
        var operations = ReadOperations("Day10\\Part1.sample.txt");
        Day10Puzzle.GetSumOfSignalStrengths(operations, 20, 40).Should().Be(13140);
    }

    [Test]
    public void GetSumOfSignalStrengths_WithRealData_ReturnsExpectedResult()
    {
        var operations = ReadOperations("Day10\\Part1.real.txt");
        Day10Puzzle.GetSumOfSignalStrengths(operations, 20, 40).Should().Be(14060);
    }

    [Test]
    public void GetCrtOutput_WithSampleData_ReturnsExpectedResult()
    {
        var operations = ReadOperations("Day10\\Part1.sample.txt");
        var crtOutput = Day10Puzzle.GetCrtOutput(operations);
        crtOutput
            .Should().Equal(
                "##..##..##..##..##..##..##..##..##..##..",
                "###...###...###...###...###...###...###.",
                "####....####....####....####....####....",
                "#####.....#####.....#####.....#####.....",
                "######......######......######......####",
                "#######.......#######.......#######....."
            );
    }

    [Test]
    public void GetCrtOutput_WithRealData_ReturnsExpectedResult()
    {
        var operations = ReadOperations("Day10\\Part1.real.txt");
        var crtOutput = Day10Puzzle.GetCrtOutput(operations);
        Console.Write(crtOutput);
        
        // Letters are PAPKFKEJ
        crtOutput
            .Should().Equal(
                "###...##..###..#..#.####.#..#.####...##.",
                "#..#.#..#.#..#.#.#..#....#.#..#.......#.",
                "#..#.#..#.#..#.##...###..##...###.....#.",
                "###..####.###..#.#..#....#.#..#.......#.",
                "#....#..#.#....#.#..#....#.#..#....#..#.",
                "#....#..#.#....#..#.#....#..#.####..##.."
            );
    }

    private Operation[] ReadOperations(string filename)
    {
        return File.ReadLines(filename).Select(ParseOperation).ToArray();
    }

    private static Operation ParseOperation(string line)
    {
        var tokens = line.Split(" ");
        var operationToken = tokens[0];

        return operationToken switch
        {
            "addx" => new AddOperation(int.Parse(tokens[1])) as Operation,
            "noop" => new NoOperation() as Operation,
            _ => throw new ArgumentOutOfRangeException($"Unrecognized operation token {tokens[0]}")
        };
    }
}