using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day14;

public static class Day14Puzzle
{
    public static int GetFinalCountOfRestingSand(Coord sandOrigin, RockPath[] rockPaths)
    {
        var cave = Cave.Create(rockPaths);

        //var i = 0;
        while (cave.AddSand(sandOrigin) is not LostToTheAbyssAddMaterialResult)
        {
            //File.WriteAllText($"tmp.cave.{(i++):0000}.txt", cave.ToString());
        }

        return cave
            .GetSandCoords()
            .Count();
    }

    public static int GetFinalCountOfRestingSandWithBedrockOnceOriginBlocked(Coord sandOrigin, RockPath[] rockPaths)
    {
        var cave = Cave.CreateWithBedrock(rockPaths);

        while (!cave.IsBlocked(sandOrigin))
        {
            cave.AddSand(sandOrigin);
        }

        return cave
            .GetSandCoords()
            .Count();
    }
}

public class Cave
{
    private readonly IDictionary<Coord, Material> _state;
    private readonly int? _bedrockDepth;

    private Cave(Dictionary<Coord, Material> state, int? bedrockDepth = null)
    {
        _state = state;
        _bedrockDepth = bedrockDepth;
    }

    public static Cave Create(RockPath[] rockPaths)
    {
        var initialState = new Dictionary<Coord, Material>();
        rockPaths.ToList().ForEach(rockPath => InitializeRockPath(rockPath, initialState));
        return new Cave(initialState);
    }

    public static Cave CreateWithBedrock(RockPath[] rockPaths)
    {
        var initialState = new Dictionary<Coord, Material>();
        rockPaths.ToList().ForEach(rockPath => InitializeRockPath(rockPath, initialState));

        var deepestRockPath = rockPaths.SelectMany(p => p.Points).Max(p => p.Y);
        var bedrockDepth = deepestRockPath + 2;

        return new Cave(initialState, bedrockDepth);
    }

    private static void InitializeRockPath(RockPath rockPath, Dictionary<Coord, Material> state)
    {
        rockPath.Points
            .Zip(rockPath.Points.Skip(1))
            .SelectMany(tuple => tuple.First.StraightLineTo(tuple.Second))
            .ToList()
            .ForEach(coord => state[coord] = Material.Rock);
    }

    public AddMaterialResult AddSand(Coord coord)
    {
        if (GetMaterialAt(coord) is not null)
            throw new InvalidOperationException($"Cannot add sand where {GetMaterialAt(coord)} already exists");

        var restingPlace = FindRestingPlaceForSandFrom(coord);
        if (restingPlace == null)
            return new LostToTheAbyssAddMaterialResult();

        _state[restingPlace] = Material.Sand;
        return new RestingPlaceAddMaterialResult(restingPlace);
    }

    private Coord? FindRestingPlaceForSandFrom(Coord coord)
    {
        var nextCoord = GetNextSandCoord(coord);

        if (nextCoord == null) // nowhere to go, so is already at rest
            return coord;

        return IsInTheAbyss(nextCoord)
            ? null
            : FindRestingPlaceForSandFrom(nextCoord);
    }

    public IEnumerable<Coord> GetSandCoords()
    {
        return GetKnownCoordsOf(Material.Sand);
    }

    private IEnumerable<Coord> GetKnownCoordsOf(Material material)
    {
        return _state
            .Where(kv => kv.Value == material)
            .Select(kv => kv.Key);
    }

    private bool IsInTheAbyss(Coord nextCoord)
    {
        return nextCoord.Y > (_bedrockDepth ?? int.MinValue) &&
               nextCoord.Y > GetKnownCoordsOf(Material.Rock).Max(rock => rock.Y);
    }

    private Coord? GetNextSandCoord(Coord coord)
    {
        var destinationsToTry = new[]
        {
            coord.Moved(Direction.Down),
            coord.Moved(Direction.Down).Moved(Direction.Left),
            coord.Moved(Direction.Down).Moved(Direction.Right)
        };
        return destinationsToTry.FirstOrDefault(destination => GetMaterialAt(destination) == null);
    }

    public override string ToString()
    {
        IEnumerable<Coord> rocks = GetKnownCoordsOf(Material.Rock).ToArray();
        var (yMin, yMax) = (rocks.Min(c => c.Y) - 5, rocks.Max(c => c.Y) + 5);
        var (xMin, xMax) = (rocks.Min(c => c.X) - 5, rocks.Max(c => c.X) + 5);
        return string.Join("\n",
            Enumerable.Range(yMin, yMax).Select(y =>
                string.Join("", Enumerable.Range(xMin, xMax).Select(x => GetSprite(new Coord(x, y))))));
    }

    private char GetSprite(Coord coord)
    {
        return GetMaterialAt(coord) switch
        {
            Material.Rock => '#',
            Material.Sand => 'o',
            null => '.',
            _ => throw new ArgumentOutOfRangeException(nameof(Material), GetMaterialAt(coord), null)
        };
    }

    private enum Material
    {
        Rock,
        Sand
    }

    public bool IsBlocked(Coord coord)
    {
        return GetMaterialAt(coord) is not null;
    }

    private Material? GetMaterialAt(Coord coord)
    {
        return coord.Y == _bedrockDepth ? Material.Rock
            : _state.ContainsKey(coord) ? _state[coord] : null;
    }
}

public class RestingPlaceAddMaterialResult : AddMaterialResult
{
    public RestingPlaceAddMaterialResult(Coord coord)
    {
    }
}

public class LostToTheAbyssAddMaterialResult : AddMaterialResult
{
}

public class AddMaterialResult
{
}

public record RockPath(params Coord[] Points);