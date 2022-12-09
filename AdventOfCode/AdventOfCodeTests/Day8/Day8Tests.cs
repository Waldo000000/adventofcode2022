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
        var treeMap = ReadTreeMap("Day8\\Part1.sample.txt");
        Day8Puzzle.CountVisibleTrees(treeMap).Should().Be(21);
    }
    
    [Test]
    public void CountVisibleTrees_WithOtherData_ReturnsExpectedResult()
    {
        var treeMap = ReadTreeMap("Day8\\Part1.other.txt");
        Day8Puzzle.CountVisibleTrees(treeMap).Should().Be(9);
    }

    [Test] public void CountVisibleTrees_WithRealData_ReturnsExpectedResult()
    {
        var treeMap = ReadTreeMap("Day8\\Part1.real.txt");
        Day8Puzzle.CountVisibleTrees(treeMap).Should().Be(1803);
    }
    
    [Test] public void GetScenicScore_WithSampleData_ReturnsExpectedResult()
    {
        var treeMap = ReadTreeMap("Day8\\Part2.sample.txt");
        Day8Puzzle.GetMaxScenicScore(treeMap).Should().Be(8);
    }

    private static TreeMap ReadTreeMap(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);
        var rows = lines.Select(line => line.ToCharArray()).Select(chars => chars.Select(CharToInt).Select(CreateTreeWithHeight).ToArray()).ToArray();
        return new TreeMap(rows);
    }

    private static int CharToInt(char ch)
    {
        return int.Parse(ch.ToString());
    }

    private static Tree CreateTreeWithHeight(int height)
    {
        return new Tree(height);
    }
}
