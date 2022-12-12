using System.Collections.Generic;
using NCalc;

namespace AdventOfCode.Day11;

public static class Day11Puzzle
{
    public static int GetLevelOfMonkeyBusiness(Monkey[] monkeys)
    {
        throw new System.NotImplementedException();
    }
}i

public record Monkey(
    int Id,
    List<int> Items,
    Expression Expression,
    int DivisibleByTest,
    int IfTestIsTrueThrowToMonkeyId,
    int IfTestIsFalseThrowToMonkeyId
);

public record Item();