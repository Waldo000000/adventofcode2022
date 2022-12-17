using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day15;

public static class Day15Puzzle
{
    public static int GetNumPositionsCannotContainBeaconInRow(int y, SensorReport[] reports)
    {
        var totalCoverage = reports
            .SelectMany(sensorReport => sensorReport.Sensor.GetKnownCoverage(sensorReport.Beacon))
            .Distinct()
            .ToArray();

        var sensorsAndBeacons = reports.SelectMany(report => new[] {report.Sensor.Coord, report.Beacon.Coord}).ToHashSet();

        return totalCoverage.Count(coord => coord.InRow(y) && !sensorsAndBeacons.Contains(coord));
    }
}

public record SensorReport(Sensor Sensor, Beacon Beacon);

public record Sensor(Coord Coord)
{
    public Coord[] GetKnownCoverage(Beacon closestBeacon)
    {
        var closestBeaconManhattanDistance = Coord.GetManhattanDistanceTo(closestBeacon.Coord);
        var allCoordsWithin = GetAllCoordsWithin(closestBeaconManhattanDistance);
        var x = allCoordsWithin.Distinct();
        return allCoordsWithin;
    }

    private Coord[] GetAllCoordsWithin(int distance)
    {
        var squareRegion = Enumerable.Range(-distance, distance * 2).SelectMany(yOffset =>
            Enumerable.Range(-distance, distance * 2)
                .Select(xOffset => new Coord(Coord.X + xOffset, Coord.Y + yOffset)));

        return squareRegion.Where(c => Coord.GetManhattanDistanceTo(c) <= distance).ToArray();
    }
};

public record Beacon(Coord Coord);