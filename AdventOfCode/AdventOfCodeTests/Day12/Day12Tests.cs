using System;
using System.IO;
using System.Linq;
using AdventOfCode.Day12;
using AdventOfCode.Day9;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day12;

public class Day12Tests
{
    private static readonly char[] EncodedHeightsAscending = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    [Test]
    public void GetFewestStepsToBestSignal_WithSampleData_ReturnsExpectedValue()
    {
        var (currentPosition, bestSignalPosition, heightmap) = ReadHeightmap("Day12\\Part1.sample.txt");

        Day12Puzzle.GetFewestStepsToBestSignal(currentPosition, bestSignalPosition, heightmap).Should().Be(31);
    }

    [Test] public void GetFewestStepsToBestSignal_WithRealData_ReturnsExpectedValue()
    {
        var (currentPosition, bestSignalPosition, heightmap) = ReadHeightmap("Day12\\Part1.real.txt");

        Day12Puzzle.GetFewestStepsToBestSignal(currentPosition, bestSignalPosition, heightmap).Should().Be(481);
    }

    private (Coord, Coord, Heightmap) ReadHeightmap(string filename)
    {
        var lines = File.ReadAllLines(filename);
        
        var row = lines
            .Select(line => line.ToCharArray())
            .ToArray();

        var currentPosition = GetCoordWithValue('S', row);
        var bestSignalPosition = GetCoordWithValue('E', row);

        var heights = row
            .Select(charMatrixRow => charMatrixRow.Select(ToHeight).ToArray())
            .ToArray();

        return (currentPosition, bestSignalPosition, new Heightmap(heights));
    }

    private Coord GetCoordWithValue(char x, char[][] array)
    {
        for (int xIdx = 0; xIdx < array.Length; xIdx++)
        {
            for (int yIdx = 0; yIdx < array.First().Length; yIdx++)
            {
                if (array[xIdx][yIdx] == x)
                    return new Coord(xIdx, yIdx);
            }
        }

        throw new Exception($"Character not found: {x}");
    }

    private int ToHeight(char ch)
    {
        return ch switch
        {
            'S' => 0,
            'E' => EncodedHeightsAscending.Length - 1,
            _ => EncodedHeightsAscending.ToList().FindIndex(c => c == ch)
        };
    }
}