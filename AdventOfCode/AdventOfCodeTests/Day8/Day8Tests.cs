using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day8;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day8;

public class Day8Tests
{
    [Test]
    public void CountVisibleTrees_WithSampleData_ReturnsExpectedResult()
    {
        var treeMap = ReadTreeMap("Day1\\Part1.sample.txt");
        Day8Puzzle.CountVisibleTrees(treeMap).Should().Be(21);
    }

    private static TreeMap ReadTreeMap(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        var rows = lines.Select(line => line.Split()).Select(chars => chars.Select(int.Parse).Select(AsTree).ToArray()).ToArray();
        return new TreeMap(rows);
    }

    private static Tree AsTree(int height)
    {
        return new Tree(height);
    }
}
