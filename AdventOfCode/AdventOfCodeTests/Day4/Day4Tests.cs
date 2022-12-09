using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day4;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day4;

public class Day4Tests
{
    [Test]
    public void DetectSubsetsInCleaningAssignments_WithSampleData_ReturnsExpectedResult()
    {
        var assignments = ReadAssignments("Day4\\Part1.sample.txt");
        Day4Puzzle.DetectSubsetsInCleaningAssignments(assignments).Should().Be(2);
    }
    
    [Test]
    public void DetectSubsetsInCleaningAssignments_WithRealData_ReturnsExpectedResult()
    {
        var assignments = ReadAssignments("Day4\\Part1.real.txt");
        Day4Puzzle.DetectSubsetsInCleaningAssignments(assignments).Should().Be(413);
    }

    [Test]
    public void DetectOverlapsInCleaningAssignments_WithSampleData_ReturnsExpectedResult()
    {
        var assignments = ReadAssignments("Day4\\Part1.sample.txt");
        Day4Puzzle.DetectOverlapsInCleaningAssignments(assignments).Should().Be(4);
    }

    [Test]
    public void DetectOverlapsInCleaningAssignments_WithRealData_ReturnsExpectedResult()
    {
        var assignments = ReadAssignments("Day4\\Part1.real.txt");
        Day4Puzzle.DetectOverlapsInCleaningAssignments(assignments).Should().Be(806);
    }
    
    private static Assignment[] ReadAssignments(string inputFilepath)
    {
        IEnumerable<string> lines = File.ReadAllLines(inputFilepath);

        return lines.Select(ReadAssignment).ToArray();
    }

    private static Assignment ReadAssignment(string line)
    {
        var sectionIdRangeStrs = line.Split(",");
        var firstPartnerSectionIds = ParseSectionIdRangeStr(sectionIdRangeStrs[0]);
        var secondPartnerSectionIds = ParseSectionIdRangeStr(sectionIdRangeStrs[1]);

        return new Assignment(firstPartnerSectionIds, secondPartnerSectionIds);
    }

    private static SectionId[] ParseSectionIdRangeStr(string sectionIdRangeStr)
    {
        var sectionIdFromToStrs = sectionIdRangeStr.Split("-");
        var fromSectionId = int.Parse(sectionIdFromToStrs[0]);
        var toSectionId = int.Parse(sectionIdFromToStrs[1]);
        return Enumerable.Range(fromSectionId, toSectionId - fromSectionId + 1).Select(i => new SectionId(i)).ToArray();
    }
}
