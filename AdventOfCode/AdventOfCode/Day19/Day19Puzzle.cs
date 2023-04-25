using System.Linq;

namespace AdventOfCode.Day19;

public static class Day19Puzzle
{
    private static readonly int Minutes = 24;

    public static int GetSumQualityLevels(Blueprint[] blueprints)
    {
        return blueprints.Sum(GetQualityLevel);
    }

    private static int GetQualityLevel(Blueprint blueprint)
    {
        return blueprint.Id * GetMaxGeodesOpenedInMinutes(blueprint, Minutes);
    }

    private static int GetMaxGeodesOpenedInMinutes(Blueprint blueprint, int minutes)
    {
        var resources = new Resources();
        var robots = new Robots(1, 0, 0, 0);
        var factory = new Factory(blueprint);
        for (int minute = 1; minute <= minutes; minute++)
        {
            (resources, var newRobots) = factory.AfterUsing(resources);
            resources = resources.AfterMiningWith(robots);
            robots += newRobots;
        }

        return resources.Geode;
    }
}

internal class Factory
{
    private readonly Blueprint _blueprint;

    public Factory(Blueprint blueprint)
    {
        _blueprint = blueprint;
    }

    public (Resources resources, Robots newRobots) AfterUsing(Resources resources)
    {
        var newRobots = new Robots();
        if (_blueprint.GeodeRobotObsidianCost <= resources.Obsidian && _blueprint.GeodeRobotOreCost <= resources.Ore)
        {
            resources = resources with
            {
                Obsidian = resources.Obsidian - _blueprint.GeodeRobotObsidianCost,
                Ore = resources.Ore - _blueprint.GeodeRobotOreCost,
            };
            newRobots += new Robots(0, 0, 0, 1);
        }

        if (resources.Clay >= _blueprint.ObsidianRobotClayCost && resources.Ore >= _blueprint.ObsidianRobotOreCost)
        {
            resources = resources with
            {
                Clay = resources.Clay - _blueprint.ObsidianRobotClayCost,
                Ore = resources.Ore - _blueprint.ObsidianRobotClayCost,
            };
            newRobots += new Robots(0, 0, 1, 0);
        }

        if (resources.Ore >= _blueprint.ClayRobotOreCost)
        {
            resources = resources with
            {
                Ore = resources.Ore - _blueprint.ClayRobotOreCost,
            };
            newRobots += new Robots(0, 1, 0, 0);
        }

        if (resources.Ore >= _blueprint.OreRobotOreCost)
        {
            resources = resources with
            {
                Ore = resources.Ore - _blueprint.OreRobotOreCost,
            };
            newRobots += new Robots(1, 0, 0, 0);
        }

        return (resources, newRobots);
    }
}

public record Robots(int Ore, int Clay, int Obsidian, int Geode)
{
    public Robots() : this(0, 0, 0, 0)
    {
    }

    public static Robots operator +(Robots a, Robots b)
    {
        return new Robots(
            a.Ore + b.Ore,
            a.Clay + b.Clay,
            a.Obsidian + b.Obsidian,
            a.Geode + b.Geode
        );
    }
};

public record Blueprint(
    int Id,
    int OreRobotOreCost,
    int ClayRobotOreCost,
    int ObsidianRobotOreCost,
    int ObsidianRobotClayCost,
    int GeodeRobotOreCost,
    int GeodeRobotObsidianCost
);

public record Resources(int Ore, int Clay, int Obsidian, int Geode)
{
    public Resources() : this(0, 0, 0, 0)
    {
    }

    public Resources AfterMiningWith(Robots robots)
    {
        return new Resources(
            Ore + robots.Ore,
            Clay + robots.Clay,
            Obsidian + robots.Obsidian,
            Geode + robots.Geode
        );
    }
}