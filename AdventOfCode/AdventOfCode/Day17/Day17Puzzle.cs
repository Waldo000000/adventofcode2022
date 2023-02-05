using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day17;

public static class Day17Puzzle
{
    public static int GetTowerHeightAfterNumRocksFallen(int numRocksFallen, JetPushPattern jetPattern)
    {
        var chamber = new Chamber(jetPattern);
        Enumerable.Range(0, numRocksFallen).ToList().ForEach(_ => chamber.AddRock());
        return chamber.TowerHeight;
    }
}

public enum JetPush
{
    Right,
    Left
}

// Note: Bottom of chamber is y=0; above chamber is y=[-ve]
public class Chamber
{
    private readonly HashSet<Coord> _fallenRocks = new();
    private readonly RockPattern _rockPattern = new();

    private readonly JetPushPattern _jetPushPattern;

    public Chamber(JetPushPattern jetPushPattern)
    {
        _jetPushPattern = jetPushPattern;
    }

    public void AddRock()
    {
        var rock = SpawnRock(_rockPattern.GetNextRock());

        while (true)
        {
            rock = Push(rock);
            var fallen = Fall(rock);
            if (fallen == rock)
                break;
            rock = fallen;
        }

        rock.Coords.ToList().ForEach(coord => _fallenRocks.Add(coord));
    }

    private Rock Fall(Rock rock)
    {
        return IsAtRest(rock) ? rock : MoveDown(rock);
    }

    public int TowerHeight => _fallenRocks.Any()
        ? _fallenRocks.Max(r => Math.Abs(r.Y))
        : 0;

    private Rock SpawnRock(Rock rockPattern)
    {
        var spawnOrigin = GetSpawnOrigin();

        var coords = rockPattern.Coords
            .Select(coord => new Coord(spawnOrigin.X + coord.X, spawnOrigin.Y + coord.Y));

        return new Rock(coords.ToArray());
    }

    private Coord GetSpawnOrigin() => new(3, -1 * (TowerHeight + 4));

    private static Rock MoveDown(Rock rock)
    {
        return new Rock(rock.Coords.Select(coord => coord.Moved(Direction.Down)).ToArray());
    }

    private Rock Push(Rock rock)
    {
        var push = _jetPushPattern.GetNextPattern();
        var moved = rock.Coords.Select(coord => coord.Moved(push.AsDirection())).ToArray();
        return moved.Any(IsCollision) ? rock : new Rock(moved);
    }

    private bool IsAtRest(Rock rock)
    {
        return rock.Coords
            .Select(coord => coord.Moved(Direction.Down))
            .Any(IsCollision);
    }

    private bool IsCollision(Coord coord)
    {
        return _fallenRocks.Contains(coord)
               || IsFloor(coord)
               || IsWall(coord);
    }

    private bool IsWall(Coord coord)
    {
        return coord.X == 0 || coord.X == 8;
    }

    private static bool IsFloor(Coord coord)
    {
        return coord.Y == 0;
    }

    public override string ToString()
    {
        return string.Join("\n", Enumerable.Range(0, TowerHeight + 8).Select(y => -1 * y).Reverse().Select(y =>
            string.Join("",
                Enumerable.Range(0, 9).Select(x =>
                {
                    if (y == 0 && (x == 0 || x == 8))
                        return "+";
                    if (x == 0 || x == 8)
                        return "|";
                    if (y == 0)
                        return "-";

                    return _fallenRocks.Contains(new Coord(x, y)) ? "#" : ".";
                }))));
    }
}

internal record RockPattern
{
    // Note: 0, 0 is lower left corner
    private static readonly Rock[] Rocks =
    {
        // ----
        new Rock(new[]
        {
            new Coord(0, 0), new Coord(1, 0), new Coord(2, 0), new Coord(3, 0)
        }),

        // +
        new Rock(new[]
        {
            new Coord(1, -2),
            new Coord(0, -1), new Coord(1, -1), new Coord(2, -1),
            new Coord(1, 0),
        }),

        // _|
        new Rock(new[]
        {
            new Coord(2, -2),
            new Coord(2, -1), new Coord(10, 0), new Coord(2, 0),
            new Coord(0, 0), new Coord(1, 0), new Coord(2, 0),
        }),

        // |
        new Rock(new[]
        {
            new Coord(0, -3),
            new Coord(0, -2),
            new Coord(0, -1),
            new Coord(0, 0),
        }),

        // 2x2 square
        new Rock(new[]
        {
            new Coord(0, -1), new Coord(1, -1),
            new Coord(0, 0), new Coord(1, 0),
        }),
    };

    private int _nextIdx;

    public Rock GetNextRock()
    {
        return Rocks[_nextIdx++ % Rocks.Length];
    }
}

public record Rock(Coord[] Coords);

public record JetPushPattern(JetPush[] Pattern)
{
    private int _nextIdx;

    public JetPush GetNextPattern()
    {
        return Pattern[_nextIdx++ % Pattern.Length];
    }
}

public static class JetPushExtensions
{
    public static Direction AsDirection(this JetPush jetPush) => jetPush switch
    {
        JetPush.Right => Direction.Right,
        JetPush.Left => Direction.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(jetPush), jetPush, null)
    };
}