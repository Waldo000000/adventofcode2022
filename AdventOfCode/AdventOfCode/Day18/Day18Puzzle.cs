using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day18;

public static class Day18Puzzle
{
    public static int GetSurfaceArea(LavaCube[] cubes)
    {
        var allCubes = cubes.ToHashSet();
        return cubes.Sum(c => GetNumFreeSides(c, allCubes));
    }

    private static int GetNumFreeSides(LavaCube cube, HashSet<LavaCube> allCubes)
    {
        var adjacentPositions = new List<LavaCube>
        {
            cube with {X = cube.X - 1},
            cube with {X = cube.X + 1},
            cube with {Y = cube.Y - 1},
            cube with {Y = cube.Y + 1},
            cube with {Z = cube.Z - 1},
            cube with {Z = cube.Z + 1}
        };
        return adjacentPositions.Count(a => !allCubes.Contains(a));
    }
}

public record LavaCube(int X, int Y, int Z);
