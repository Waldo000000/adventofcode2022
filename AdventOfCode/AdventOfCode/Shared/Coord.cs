using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Shared;

/// <param name="X">Distance to the right</param>
/// <param name="Y">Distance down</param>
public record Coord(int X, int Y)
{
    public override string ToString()
    {
        return string.Concat("(", X, ",", Y, ")");
    }
};

public static class CoordExtensions
{
    public static Coord Moved(this Coord coord, Direction direction)
    {
        return direction switch
        {
            Direction.Up => coord with {Y = coord.Y - 1},
            Direction.Down => coord with {Y = coord.Y + 1},
            Direction.Left => coord with {X = coord.X - 1},
            Direction.Right => coord with {X = coord.X + 1},
            _ => throw new ArgumentOutOfRangeException($"Unrecognized direction {direction}")
        };
    }

    public static bool IsXAdjacent(this Coord fromCoord, Coord toCoord)
    {
        return Math.Abs(fromCoord.X - toCoord.X) <= 1;
    }

    public static bool IsYAdjacent(this Coord fromCoord, Coord toCoord)
    {
        return Math.Abs(fromCoord.Y - toCoord.Y) <= 1;
    }

    /// <summary>
    /// Returns all intermediary coords (inclusive of ends) in a straight horizontal or vertical line 
    /// </summary>
    public static IEnumerable<Coord> StraightLineTo(this Coord source, Coord destination)
    {
        if (source.X == destination.X)
            return source.Y.RangeUntil(destination.Y).Select(y => source with {Y = y});

        if (source.Y == destination.Y)
            return source.X.RangeUntil(destination.X).Select(x => source with {X = x});

        throw new InvalidOperationException(
            $"Cannot calculate intermediary coords between coords that are neither in the same horizontal nor vertical plane ({source} and {destination})");
    }
}