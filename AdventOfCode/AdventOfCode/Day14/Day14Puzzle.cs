using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day14;

public static class Day14Puzzle
{
    public static int GetFinalCountOfRestingSand(Coord sandOrigin, RockPath[] rockPaths)
    {
        var cave = Cave.Create(rockPaths);

        while (cave.AddSand(sandOrigin) is not LostToTheAbyssAddMaterialResult)
        {
        }

        return cave
            .GetSandCoords()
            .Count();
    }
}

public class Cave
{
    private readonly IDictionary<Coord, Material> _state;

    private Cave(Dictionary<Coord, Material> state)
    {
        _state = state;
    }

    public static Cave Create(RockPath[] rockPaths)
    {
        var initialState = new Dictionary<Coord, Material>();
        rockPaths.ToList().ForEach(rockPath => InitializeRockPath(rockPath, initialState));
        return new Cave(initialState);
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
        if (_state.ContainsKey(coord))
            throw new InvalidOperationException($"Cannot add sand where {_state[coord]} already exists");

        var restingPlace = FindRestingPlaceForSandFrom(coord);
        if (restingPlace == null)
            return new LostToTheAbyssAddMaterialResult();

        _state[restingPlace] = Material.Sand;
        return new RestingPlaceAddMaterialResult(restingPlace);
    }

    private Coord? FindRestingPlaceForSandFrom(Coord coord)
    {
        var nextCoord = GetNextSandCoord(coord);

        if (nextCoord == null)
            return coord;

        return IsInTheAbyss(nextCoord)
            ? null
            : FindRestingPlaceForSandFrom(nextCoord);
    }

    public IEnumerable<Coord> GetSandCoords()
    {
        return GetAllCoordsOf(Material.Sand);
    }

    private IEnumerable<Coord> GetAllCoordsOf(Material material)
    {
        return _state
            .Where(kv => kv.Value == material)
            .Select(kv => kv.Key);
    }

    private bool IsInTheAbyss(Coord nextCoord)
    {
        var rockBottom = GetAllCoordsOf(Material.Rock).Max(rock => rock.Y);
        return nextCoord.Y > rockBottom;
    }

    private Coord? GetNextSandCoord(Coord coord)
    {
        var destinationsToTry = new[]
        {
            coord.Moved(Direction.Down),
            coord.Moved(Direction.Down).Moved(Direction.Left),
            coord.Moved(Direction.Down).Moved(Direction.Right)
        };
        return destinationsToTry.FirstOrDefault(destination => !_state.ContainsKey(destination));
    }

    private enum Material
    {
        Rock,
        Sand
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