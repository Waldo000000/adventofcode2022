using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day11;
using FluentAssertions;
using NCalc;
using NUnit.Framework;

namespace AdventOfCodeTests.Day11;

public class Day11Tests
{
    [Test]
    public void GetLevelOfMonkeyBusiness_After20RoundsWithSampleData_ReturnsExpectedValues()
    {
        var monkeys = ReadMonkeys("Day11\\Part1.sample.txt").ToArray();

        Day11Puzzle.GetLevelOfMonkeyBusiness(monkeys).Should().Be(10605);
    }

    private IEnumerable<Monkey> ReadMonkeys(string filename)
    {
        return File.ReadAllLines(filename)
            .Chunk(7)
            .Select(ch => ParseMonkey(ch))
            .ToList();
    }

    private Monkey ParseMonkey(string[] lines)
    {
        var id = int.Parse(lines[0].Split("Monkey ")[1].Trim(':'));
        var items = lines[1].Split(": ")[1].Split(", ").Select(int.Parse).ToList();
        var operation = new Expression(lines[2].Split("new = ")[1]);
        var divisibleByTest = int.Parse(lines[3].Split("divisible by ")[1]);
        var ifTestIsTrueThrowToMonkeyId = int.Parse(lines[4].Split("throw to monkey ")[1]);
        var ifTestIsFalseThrowToMonkeyId = int.Parse(lines[5].Split("throw to monkey ")[1]);

        return new Monkey(
            id,
            items,
            operation,
            divisibleByTest,
            ifTestIsTrueThrowToMonkeyId,
            ifTestIsFalseThrowToMonkeyId
        );
    }
}