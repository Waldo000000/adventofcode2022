using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day10;
using AdventOfCode.Day11;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day11;

public class Day11Tests
{
    [Test]
    public void GetLevelOfMonkeyBusiness_After20RoundsWithSampleData_ReturnsExpectedValues()
    {
        var monkeys = ReadMonkeys("Day11\\Part1.sample.txt");

        Day11Puzzle.GetLevelOfMonkeyBusiness(monkeys).Should().Be(10605);
    }

    private Monkey[] ReadMonkeys(string filename)
    {
        // TODO:
        // File.ReadLines(filename).Select(ParseMonkey).ToArray();
        return new[]
        {
            new Monkey()
        };
    }
}