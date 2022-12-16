using System;

namespace AdventOfCode.Shared;

public record Coord(int X, int Y);

public static class CoordExtensions
{
    public static Coord Moved(this Coord coord, Direction direction)
    {
        return direction switch
        {
            Direction.Up => coord with {Y = coord.Y + 1},
            Direction.Down => coord with {Y = coord.Y - 1},
            Direction.Left => coord with {X = coord.X - 1},
            Direction.Right => coord with {X = coord.X + 1},
            _ => throw new ArgumentOutOfRangeException($"Unrecognized direction {direction}")
        };
    }

    public static bool IsAdjacent(this Coord firstCoord, Coord secondCoord)
    {
        return Math.Abs(firstCoord.X - secondCoord.X) <= 1 &&
               Math.Abs(firstCoord.Y - secondCoord.Y) <= 1;
    }

    public static bool IsXAdjacent(this Coord fromCoord, Coord toCoord)
    {
        return Math.Abs(fromCoord.X - toCoord.X) <= 1;
    }

    public static bool IsYAdjacent(this Coord fromCoord, Coord toCoord)
    {
        return Math.Abs(fromCoord.Y - toCoord.Y) <= 1;
    }

    public static Direction? GetXDirection(this Coord fromCoord, Coord toCoord)
    {
        var xDelta = toCoord.X - fromCoord.X;
        if (xDelta < 0)
            return Direction.Left;
        if (xDelta > 0)
            return Direction.Right;
        return null;
    }

    public static Direction? GetYDirection(this Coord fromCoord, Coord toCoord)
    {
        var yDelta = toCoord.Y - fromCoord.Y;
        if (yDelta < 0)
            return Direction.Down;
        if (yDelta > 0)
            return Direction.Up;
        return null;
    }
}