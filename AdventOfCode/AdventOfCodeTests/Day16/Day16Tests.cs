using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day16;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day16;

public class Day16Tests
{
    private readonly Regex _valveRegex = new(@"Valve ([\w]+) has flow rate=([\d]+); tunnels? leads? to valves? (.*)$");

    [Test]
    public void GetMaxPressureReleaseWithinMinutes_WithSampleData_ReturnsExpectedValue()
    {
        var valves = ReadValves("Day16\\Part1.sample.txt").ToDictionary(v => v.Id);
        Day16Puzzle.GetMaxPressureReleaseWithinMinutes(30, "AA", valves, 1).Should().Be(1651);
    }

    [Test]
    public void GetMaxPressureReleaseWithinMinutes_WithRealData_ReturnsExpectedValue()
    {
        var valves = ReadValves("Day16\\Part1.real.txt").ToDictionary(v => v.Id);
        Day16Puzzle.GetMaxPressureReleaseWithinMinutes(30, "AA", valves, 1).Should().Be(2181);
    }

    [Test]
    public void GetMaxPressureReleaseWithinMinutesWithTwoActors_WithSampleData_ReturnsExpectedValue()
    {
        var valves = ReadValves("Day16\\Part1.sample.txt").ToDictionary(v => v.Id);
        Day16Puzzle.GetMaxPressureReleaseWithinMinutes(26, "AA", valves, 2).Should().Be(1707);
    }

    [Test]
    public void GetMaxPressureReleaseWithinMinutesWithTwoActors_WithRealData_ReturnsExpectedValue()
    {
        var valves = ReadValves("Day16\\Part1.real.txt").ToDictionary(v => v.Id);
        Day16Puzzle.GetMaxPressureReleaseWithinMinutes(26, "AA", valves, 2).Should().Be(2824);
    }

    private Valve[] ReadValves(string filename)
    {
        return File.ReadLines(filename)
            .Select(ReadValve)
            .ToArray();
    }

    private Valve ReadValve(string line)
    {
        var groups = _valveRegex.Match(line).Groups;
        return new Valve(
            groups[1].ToString(),
            int.Parse(groups[2].ToString()),
            groups[3].ToString().Split(", ")
        );
    }
}