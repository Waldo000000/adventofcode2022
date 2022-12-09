using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day1;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day1;

public class Day1Tests
{
    [Test]
    public void GetMaxCaloriesFromOneElf_WithRealData_ReturnsExpectedValue()
    {
        var elves = ReadElves("Day1\\input.txt");
        Day1Puzzle.GetMaxCaloriesFromOneElf(elves.ToList()).Should().Be(69912);
    }

    private static IEnumerable<Elf> ReadElves(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadLines(inputFilepath);
        List<List<string>> groups = ToGroups(lines);
        return groups.Select(ToElf).ToList();
    }

    private static Elf ToElf(List<string> calorieLines)
    {
        return new Elf(calorieLines.Select(ToFood).ToList());
    }

    private static Food ToFood(string line)
    {
        return new Food(int.Parse(line));
    }

    private static List<List<string>> ToGroups(IEnumerable<string> lines)
    {
        var groups = new List<List<string>>();
        var linesInGroup = new List<string>();
        foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                groups.Add(linesInGroup);
                linesInGroup = new List<string>();
                continue;
            }

            linesInGroup.Add(line);
        }

        return groups;
    }
}
