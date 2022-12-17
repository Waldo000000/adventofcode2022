using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day15;

public static class Day15Puzzle
{
    public static long GetDistressBeaconTuningFrequency(int maxCoordinateValue, SensorReport[] sensorReports)
    {
        var distressBeaconCoord = FindDistressBeacon(maxCoordinateValue, sensorReports);

        return distressBeaconCoord.X * 4000000L + distressBeaconCoord.Y;
    }

    private static Coord FindDistressBeacon(int maxCoordinateValue, SensorReport[] sensorReports)
    {
        var candidates = sensorReports
            .SelectMany(report =>
                report.GetCoordsJustOutsideCoverageBoundary()
                    .Where(coord => IsInSearchArea(coord, maxCoordinateValue)));

        var sensorsAndBeacons = GetSensorsAndBeacons(sensorReports);
        var distressBeaconCoords = candidates.Where(coord =>
            !sensorReports.Any(report => report.IsInRange(coord)) && !sensorsAndBeacons.Contains(coord));

        return distressBeaconCoords.Distinct().Single();
    }

    private static bool IsInSearchArea(Coord coord, int maxCoordinateValue)
    {
        return coord.X >= 0 && coord.X <= maxCoordinateValue && coord.Y >= 0 && coord.Y <= maxCoordinateValue;
    }

    public static int GetNumPositionsCannotContainKnownBeaconInRow(int y, SensorReport[] reports)
    {
        var sensorsAndBeacons = GetSensorsAndBeacons(reports);

        var xCoverageInRow = GetXCoverageInRow(y, reports);

        return xCoverageInRow.Distinct().Count(x => !sensorsAndBeacons.Contains(new Coord(x, y)));
    }

    private static HashSet<Coord> GetSensorsAndBeacons(SensorReport[] reports)
    {
        return reports.SelectMany(report => new[] {report.Sensor.Coord, report.Beacon.Coord}).ToHashSet();
    }

    private static IEnumerable<int> GetXCoverageInRow(int y, SensorReport[] reports)
    {
        return reports.SelectMany(report => report.GetXCoverageInRow(y));
    }
}

public record SensorReport(Sensor Sensor, Beacon Beacon)
{
    public bool IsInRange(Coord coord)
    {
        return Sensor.Coord.GetManhattanDistanceTo(coord) <= Sensor.Coord.GetManhattanDistanceTo(Beacon.Coord);
    }

    public int[] GetXCoverageInRow(int y)
    {
        var distanceToBeacon = Sensor.Coord.GetManhattanDistanceTo(Beacon.Coord);
        var distanceToRow = Math.Abs(Sensor.Coord.Y - y);

        var remainingDistance = distanceToBeacon - distanceToRow;
        if (remainingDistance <= 0)
            return Array.Empty<int>();

        var xCoverage = Enumerable.Range(Sensor.Coord.X - remainingDistance, remainingDistance * 2 + 1);
        return xCoverage.ToArray();
    }

    public IEnumerable<Coord> GetCoordsJustOutsideCoverageBoundary()
    {
        var radius = Sensor.Coord.GetManhattanDistanceTo(Beacon.Coord);
        foreach (var yOffset in Enumerable.Range(-1 * radius, radius * 2 + 1))
        {
            // just beyond left boundary
            yield return new Coord(Sensor.Coord.X - (radius - Math.Abs(yOffset) - 1), Sensor.Coord.Y + yOffset);

            // just beyond right boundary
            yield return new Coord(Sensor.Coord.X + (radius - Math.Abs(yOffset) + 1), Sensor.Coord.Y + yOffset);
        }
    }
}

public record Sensor(Coord Coord);

public record Beacon(Coord Coord);