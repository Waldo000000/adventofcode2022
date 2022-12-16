using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Shared;
using Expression = NCalc.Expression;

namespace AdventOfCode.Day11;

public static class Day11Puzzle
{
    public static long GetLevelOfMonkeyBusinessAfterRounds(Monkey[] monkeys, long numRounds, IReliefStrategy reliefStrategy)
    {
        for (long round = 0; round < numRounds; round++)
        {
            monkeys.ToList().ForEach(monkey => { monkey.InspectItemsThenThrowTo(monkeys, reliefStrategy); });
        }

        return GetMonkeyBusiness(monkeys);
    }

    private static long GetMonkeyBusiness(Monkey[] monkeys)
    {
        return monkeys
            .OrderByDescending(m => m.NumInspectedItems)
            .Take(2)
            .Select(m => m.NumInspectedItems)
            .Aggregate((x, y) => x * y);
    }
}

public record Monkey(
    long Id,
    List<Item> Items,
    Expression NewWorryLevelAfterInspectionOperation,
    long NewWorryLevelIsDivisibleByTest,
    long IfTestIsTrueThrowToMonkeyId,
    long IfTestIsFalseThrowToMonkeyId
)
{
    private const string OldWorryLevelExpressionName = "old";
    public long NumInspectedItems { get; private set; }

    public void InspectItemsThenThrowTo(Monkey[] monkeys, IReliefStrategy reliefStrategy)
    {
        var oldItems = Items.ToList();
        oldItems.ToList().ForEach(item => InspectItemThenThrowTo(monkeys, item, reliefStrategy));
    }

    private void InspectItemThenThrowTo(Monkey[] monkeys, Item item, IReliefStrategy reliefStrategy)
    {
        NumInspectedItems++;
        UpdateWorryLevelAfterInspection(item);
        reliefStrategy.AdjustWorryLevel(item, monkeys);
        var toMonkeyId = GetThrowToMonkeyId(item);
        ThrowItemTo(monkeys, toMonkeyId, item);
    }

    private void UpdateWorryLevelAfterInspection(Item item)
    {
        NewWorryLevelAfterInspectionOperation.Parameters[OldWorryLevelExpressionName] = item.WorryLevel;
        item.WorryLevel = (long) NewWorryLevelAfterInspectionOperation.Evaluate();
    }

    private long GetThrowToMonkeyId(Item item)
    {
        return item.WorryLevel % NewWorryLevelIsDivisibleByTest == 0
            ? IfTestIsTrueThrowToMonkeyId
            : IfTestIsFalseThrowToMonkeyId;
    }

    private void ThrowItemTo(Monkey[] monkeys, long toMonkeyId, Item item)
    {
        var toMonkey = GetMonkeyWithId(monkeys, toMonkeyId);

        toMonkey.Items.Add(item);
        Items.Remove(item);
    }

    private static Monkey GetMonkeyWithId(Monkey[] monkeys, long toMonkeyId)
    {
        return monkeys.Single(m => m.Id == toMonkeyId);
    }
}

public interface IReliefStrategy
{
    void AdjustWorryLevel(Item item, Monkey[] monkeys);
}

public class ReduceWorryStrategy : IReliefStrategy
{
    public void AdjustWorryLevel(Item item, Monkey[] monkeys)
    {
        item.WorryLevel /= 3;
    }
}

public class ModuloReduceWorryStrategy : IReliefStrategy
{
    // Copied from https://github.com/TomPeters/advent-of-code-2022/blob/main/AdventOfCode/AdventOfCode/Day11/Day11Puzzle.cs#L12
    public void AdjustWorryLevel(Item item, Monkey[] monkeys)
    {
        var productOfAllDivisors = monkeys.ToList().Select(m => m.NewWorryLevelIsDivisibleByTest).Product();
        item.WorryLevel %= productOfAllDivisors;
    }
}

public class Item
{
    public Item(long worryLevel)
    {
        WorryLevel = worryLevel;
    }

    public long WorryLevel { get; set; }
}