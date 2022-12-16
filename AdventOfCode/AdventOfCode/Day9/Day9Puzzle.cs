using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared;

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

public record Motion(Direction Direction, int NumSteps);
