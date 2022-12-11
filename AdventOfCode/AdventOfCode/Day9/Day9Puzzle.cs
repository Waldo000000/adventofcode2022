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
        Position = Position.Moved(direction);
        
        if (NextKnot == null) return;

        NextKnot.Follow(Position);
    }

    private void Follow(Coord target)
    {
        Position = GetNewPositionAfterFollowing(target);

        if (NextKnot == null) return;

        NextKnot.Follow(Position);
    }

    private Coord GetNewPositionAfterFollowing(Coord target)
    {
        if (!Position.IsXAdjacent(target))
        {
            if (!Position.IsYAdjacent(target))
            {
                return new Coord(
                    target.X < Position.X ? Position.X - 1 : Position.X + 1,
                    target.Y < Position.Y ? Position.Y - 1 : Position.Y + 1
                );
            }

            return new Coord(
                target.X < Position.X ? Position.X - 1 : Position.X + 1,
                target.Y != Position.Y ? target.Y : Position.Y
            );
        }

        if (!Position.IsYAdjacent(target))
        {
            return new Coord(
                target.X != Position.X ? target.X : Position.X,
                target.Y < Position.Y ? Position.Y - 1 : Position.Y + 1
            );
        }

        return Position;
    }

    public Coord Position { get; private set; } = Day9Puzzle.Origin;
}

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

public record Coord(int X, int Y);

public record Motion(Direction Direction, int NumSteps);

public enum Direction
{
    Up,

    // UpLeft,
    // UpRight,
    Down,

    // DownLeft,
    // DownRight,
    Left,
    Right
}