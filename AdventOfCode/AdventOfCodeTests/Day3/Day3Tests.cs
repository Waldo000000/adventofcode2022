using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day3;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day3;

public class Day3Tests
{
    [Test]
    public void GetSumOfPackingErrorPriorities_WithSampleData_ReturnsExpectedResult()
    {
        var rucksacks = ReadRucksacks("Day3\\Part1.sample.txt");
        Day3Puzzle.GetSumOfPackingErrorPriorities(rucksacks).Should().Be(157);
    }
    
    [Test]
    public void GetSumOfPackingErrorPriorities_WithRealData_ReturnsExpectedResult()
    {
        var rucksacks = ReadRucksacks("Day3\\Part1.real.txt");
        Day3Puzzle.GetSumOfPackingErrorPriorities(rucksacks).Should().Be(7872);
    }
    
    private static Rucksack[] ReadRucksacks(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);

        return lines.Select(ReadRucksack).ToArray();
    }

    private static Rucksack ReadRucksack(string line)
    {
        var items = line.ToCharArray().Select(ch => new Item(ch.ToString())).ToList();

        return new Rucksack(new[]
        {
            new Compartment(items.Take(items.Count/2).ToArray()),
            new Compartment(items.Skip(items.Count/2).ToArray())
        });
    }
}
