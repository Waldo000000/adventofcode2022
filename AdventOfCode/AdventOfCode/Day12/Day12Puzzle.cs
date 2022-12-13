using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Day9;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode.Day12;

public static class Day12Puzzle
{
    public static int GetFewestStepsFromLowestElevationToBestSignal(Coord bestSignalPosition,
        Heightmap heightmap)
    {
        var (nodeIds, graph) = CreateGraph(heightmap);
        var lowestElevationCoords = heightmap.GetCoordsWithHeight(0);
        return lowestElevationCoords
            .Select(lowestElevationCoord =>
            {
                var from = nodeIds[lowestElevationCoord];
                var to = nodeIds[bestSignalPosition];
                var result = graph.Dijkstra(from, to);
                return result.Distance;
            })
            .MinBy(distance => distance);
    }

    public static int GetFewestStepsToBestSignal(Coord currentPosition, Coord bestSignalPosition, Heightmap heightmap)
    {
        var (nodeIds, graph) = CreateGraph(heightmap);
        var from = nodeIds[currentPosition];
        var to = nodeIds[bestSignalPosition];
        var result = graph.Dijkstra(from, to);
        return result.Distance;
    }

    private static (Dictionary<Coord, uint> nodeIds, Graph graph) CreateGraph(Heightmap heightmap)
    {
        var graph = new Graph();
        var nodeIds = CreateNodes(heightmap, graph);

        Enumerable.Range(0, heightmap.GetMaxX()).ToList().ForEach(xIdx =>
        {
            Enumerable.Range(0, heightmap.GetMaxY()).ToList().ForEach(yIdx =>
            {
                var fromCoord = new Coord(xIdx, yIdx);

                heightmap
                    .GetAdjacentCoords(fromCoord)
                    .Where(adjCoord => heightmap.CanTraverse(fromCoord, adjCoord))
                    .ToList().ToList().ForEach(accessibleCoord =>
                    {
                        graph.Connect(nodeIds[fromCoord], nodeIds[accessibleCoord], 1);
                    });
            });
        });

        return (nodeIds, graph);
    }

    private static Dictionary<Coord, uint> CreateNodes(Heightmap heightmap, Graph graph)
    {
        var nodeIds = new Dictionary<Coord, uint>();

        Enumerable.Range(0, heightmap.GetMaxX()).ToList().ForEach(xIdx =>
        {
            Enumerable.Range(0, heightmap.GetMaxY()).ToList().ForEach(yIdx =>
            {
                var nodeKey = graph.AddNode();
                nodeIds[new Coord(xIdx, yIdx)] = nodeKey;
            });
        });

        return nodeIds;
    }
}

public record Heightmap(int[][] Heights)
{
    public bool CanTraverse(Coord from, Coord to)
    {
        return GetHeight(to) <= GetHeight(from) + 1;
    }

    public IEnumerable<Coord> GetAdjacentCoords(Coord from)
    {
        if (from.X > 0)
            yield return new Coord(from.X - 1, from.Y);
        if (from.Y < GetMaxY() - 1)
            yield return new Coord(from.X, from.Y + 1);
        if (from.X < GetMaxX() - 1)
            yield return new Coord(from.X + 1, from.Y);
        if (from.Y > 0)
            yield return new Coord(from.X, from.Y - 1);
    }

    public IEnumerable<Coord> GetCoordsWithHeight(int height)
    {
        var matches = new List<Coord>();
        Enumerable.Range(0, GetMaxX()).ToList().ForEach(xIdx =>
        {
            var ints = Enumerable.Range(0, GetMaxY()).ToList();
            ints.ForEach(yIdx =>
            {
                if (GetHeight(xIdx, yIdx) == height)
                {
                    matches.Add(new Coord(xIdx, yIdx));
                }
            });
        });
        return matches;
    }

    private int GetHeight(Coord coord)
    {
        return GetHeight(coord.X, coord.Y);
    }

    private int GetHeight(int x, int y)
    {
        return Heights[x][y];
    }

    public int GetMaxY() => Heights.First().Length;

    public int GetMaxX() => Heights.Length;
};