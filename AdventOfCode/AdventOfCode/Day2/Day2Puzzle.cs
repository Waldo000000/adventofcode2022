using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day2;

public static class Day2Puzzle
{
    private static readonly Dictionary<string, HandShape> StrategyCipher = new()
    {
        {"A", HandShape.Rock},
        {"B", HandShape.Paper},
        {"C", HandShape.Scissors},

        {"X", HandShape.Rock},
        {"Y", HandShape.Paper},
        {"Z", HandShape.Scissors},
    };

    public static int GetTotalScore(StrategyGuide strategyGuide)
    {
        return 0;
    }

    public static StrategyGuide DecryptStrategyGuide(IEnumerable<string> lines)
    {
        var turns = lines.Select(line =>
        {
            var tokens = line.Split(" ");
            return new Turn(StrategyCipher[tokens[0]], StrategyCipher[tokens[1]]);
        }).ToList();
        return new StrategyGuide(turns);
    }
}

public record StrategyGuide(List<Turn> Turns);

public record Turn(HandShape OpponentTurn, HandShape PlayerTurn);

public enum HandShape
{
    Rock,
    Paper,
    Scissors
}