using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day15;
using AdventOfCode.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day15;

public class Day15Tests
{
    private static readonly Regex SensorReportRegex =
        new Regex(@"Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)");

    [Test]
    public void GetNumPositionsCannotContainBeaconInRow_WithSampleData_ReturnsExpectedValue()
    {
        var sensorReports = ReadSensorReports("Day15\\Part1.sample.txt").ToArray();

        Day15Puzzle.GetNumPositionsCannotContainBeaconInRow(10, sensorReports).Should().Be(26);
    }
    
    [Test]
    public void GetNumPositionsCannotContainBeaconInRow_WithRealData_ReturnsExpectedValue()
    {
        var sensorReports = ReadSensorReports("Day15\\Part1.real.txt").ToArray();

        Day15Puzzle.GetNumPositionsCannotContainBeaconInRow(2000000, sensorReports).Should().Be(5040643);
    }

    private SensorReport[] ReadSensorReports(string filename)
    {
        return File.ReadAllLines(filename)
            .Select(ReadSensorReport)
            .ToArray();
    }

    private SensorReport ReadSensorReport(string line)
    {
        var groups = SensorReportRegex.Match(line).Groups;
        return new SensorReport(
            new Sensor(new Coord(
                int.Parse(groups[1].ToString()),
                int.Parse(groups[2].ToString()))
            ),
            new Beacon(new Coord(
                int.Parse(groups[3].ToString()),
                int.Parse(groups[4].ToString())
            ))
        );
    }
}