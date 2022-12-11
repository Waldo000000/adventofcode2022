using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day9;

public static class Day9Puzzle
{
    public static readonly Coord Origin = new(0, 0);

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
    public Knot TopKnot { get; }
    public Coord TailPosition
    {
        get
        {
            var knot = TopKnot;
            while (knot.NextKnot != null)
            {
                knot = knot.NextKnot;
            }

            return knot.Position;
        }
    }

    public Rope(Knot topKnot)
    {
        TopKnot = topKnot;
    }

    
    public static Rope Create(int numKnots)
    {
        var topKnot = new Knot();
        var head = topKnot;
        for (int i = 0; i < numKnots - 1; i++)
        {
            var nextKnot = new Knot();
            head.RegisterNextKnot(nextKnot);
            head = nextKnot;
        }

        return new Rope(topKnot);
    }

    public void Move(Direction direction)
    {
        TopKnot.Move(direction);
    }
}

public class Knot
{
    public Knot? NextKnot { get; private set; }

    public void RegisterNextKnot(Knot? nextKnot)
    {
        NextKnot = nextKnot;
    }
    
    public void Move(Direction direction)
    {
        var newPosition = Position.Moved(direction);
        if (NextKnot != null && !NextKnot.Position.IsAdjacent(newPosition))
            NextKnot.Position = Position;
        Position = newPosition;
    }

    public Coord Position { get; private set; } = Day9Puzzle.Origin;
}

public static class CoordExtensions
{
    public static Coord Moved(this Coord coord, Direction direction)
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