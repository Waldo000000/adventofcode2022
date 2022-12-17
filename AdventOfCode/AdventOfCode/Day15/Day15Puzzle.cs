using System;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day15;

public static class Day15Puzzle
{
    public static int GetNumPositionsCannotContainBeaconInRow(int y, SensorReport[] reports)
    {
        var sensorsAndBeacons = reports.SelectMany(report => new[] {report.Sensor.Coord, report.Beacon.Coord}).ToHashSet();

        var coverageInRow = reports
            .SelectMany(report => report.GetCoverageInRow(y))
            .Distinct();

        return coverageInRow.Count(coord => !sensorsAndBeacons.Contains(coord));
    }
}

public record SensorReport(Sensor Sensor, Beacon Beacon)
{
    public Coord[] GetCoverageInRow(int y)
    {
        var distanceToBeacon = Sensor.Coord.GetManhattanDistanceTo(Beacon.Coord);
        var distanceToRow = Math.Abs(Sensor.Coord.Y - y);

        var remainingDistance = distanceToBeacon - distanceToRow;
        if (remainingDistance <= 0)
            return new Coord[]{};

        var xCoverage = Enumerable.Range(Sensor.Coord.X - remainingDistance, remainingDistance * 2 + 1);
        return xCoverage.Select(x => new Coord(x, y)).ToArray();
    }
}

public record Sensor(Coord Coord);

public record Beacon(Coord Coord);