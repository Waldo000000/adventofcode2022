using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day19;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day19;

public class Day19Tests
{
    [Test]
    public void GetSumQualityLevels_WithSampleData_ReturnsExpectedValue()
    {
        var blueprints = ReadBlueprints("Day19\\Part1.sample.txt").ToArray();

        Day19Puzzle.GetSumQualityLevels(blueprints).Should().Be(33);
    }

    private Blueprint[] ReadBlueprints(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var integerPattern = new Regex(@"\d+");
        List<Blueprint> list = new List<Blueprint>();
        foreach (var line in lines)
        {
            var integers = integerPattern.Matches(line)
                .Select(m => int.Parse(m.ToString()))
                .ToList();

            var blueprint = new Blueprint(
                integers.ElementAt(0),
                integers.ElementAt(1),
                integers.ElementAt(2),
                integers.ElementAt(3),
                integers.ElementAt(4),
                integers.ElementAt(5),
                integers.ElementAt(6)
            );

            list.Add(blueprint);
        }

        return list.ToArray();
    }
}