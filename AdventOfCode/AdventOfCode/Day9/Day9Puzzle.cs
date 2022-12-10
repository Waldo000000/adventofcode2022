using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day9;

public static class Day9Puzzle
{
    public static int GetNumberOfUniqueTailPositions(Motion[] motions, Rope rope)
    {
        var tailPositions = new HashSet<Coord>();
        motions.ToList().ForEach(motion =>
        {
            Enumerable.Range(0, motion.NumSteps).ToList().ForEach(_ =>
            {
                rope.Move(motion.Direction);
                tailPositions.Add(rope.TailPosition);
            });
        });
        return tailPositions.Count;
    }
}

public class Rope
{
    private static readonly Coord Origin = new(0, 0);

    public void Move(Direction direction)
    {
        var newHeadPosition = HeadPosition.Move(direction);
        if (!TailPosition.IsAdjacent(newHeadPosition))
            TailPosition = HeadPosition;
        HeadPosition = newHeadPosition;
    }

    public Coord HeadPosition { get; private set; } = Origin;
    public Coord TailPosition { get; private set; } = Origin;
}

public static class CoordExtensions
{
    public static Coord Move(this Coord coord, Direction direction)
    {
        return direction switch
        {
            Direction.Up => coord with {y = coord.y + 1},
            Direction.Down => coord with {y = coord.y - 1},
            Direction.Left => coord with {x = coord.x - 1},
            Direction.Right => coord with {x = coord.x + 1},
            _ => throw new ArgumentOutOfRangeException($"Unrecognized direction {direction}")
        };
    }

    public static bool IsAdjacent(this Coord firstCoord, Coord secondCoord)
    {
        return Math.Abs(firstCoord.x - secondCoord.x) <= 1 &&
               Math.Abs(firstCoord.y - secondCoord.y) <= 1;
    }
}

public record Coord(int x, int y);

public record Motion(Direction Direction, int NumSteps);

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}