using System.Collections.Generic;
using System.Linq;
using Expression = NCalc.Expression;

namespace AdventOfCode.Day11;

public static class Day11Puzzle
{
    public static int GetLevelOfMonkeyBusinessAfterRounds(Monkey[] monkeys, int numRounds)
    {
        for (int round = 0; round < numRounds; round++)
        {
            monkeys.ToList().ForEach(monkey => { monkey.InspectItemsThenThrowTo(monkeys); });
        }

        return GetMonkeyBusiness(monkeys);
    }

    private static int GetMonkeyBusiness(Monkey[] monkeys)
    {
        return monkeys
            .OrderByDescending(m => m.NumInspectedItems)
            .Take(2)
            .Select(m => m.NumInspectedItems)
            .Aggregate((x, y) => x * y);
    }
}

public record Monkey(
    int Id,
    List<Item> Items,
    Expression NewWorryLevelAfterInspectionOperation,
    int NewWorryLevelIsDivisibleByTest,
    int IfTestIsTrueThrowToMonkeyId,
    int IfTestIsFalseThrowToMonkeyId
)
{
    private const string OldWorryLevelExpressionName = "old";
    public int NumInspectedItems { get; private set; }

    public void InspectItemsThenThrowTo(Monkey[] monkeys)
    {
        var oldItems = Items.ToList();
        oldItems.ToList().ForEach(item => InspectItemThenThrowTo(monkeys, item));
    }

    private void InspectItemThenThrowTo(Monkey[] monkeys, Item item)
    {
        NumInspectedItems++;
        UpdateWorryLevelAfterInspection(item);
        var toMonkeyId = GetThrowToMonkeyId(item);
        ThrowItemTo(monkeys, toMonkeyId, item);
    }

    private void UpdateWorryLevelAfterInspection(Item item)
    {
        NewWorryLevelAfterInspectionOperation.Parameters[OldWorryLevelExpressionName] = item.WorryLevel;
        var newWorryLevel = (int) NewWorryLevelAfterInspectionOperation.Evaluate();
        item.WorryLevel = newWorryLevel / 3;
    }

    private int GetThrowToMonkeyId(Item item)
    {
        return item.WorryLevel % NewWorryLevelIsDivisibleByTest == 0
            ? IfTestIsTrueThrowToMonkeyId
            : IfTestIsFalseThrowToMonkeyId;
    }

    private void ThrowItemTo(Monkey[] monkeys, int toMonkeyId, Item item)
    {
        var toMonkey = GetMonkeyWithId(monkeys, toMonkeyId);

        toMonkey.Items.Add(item);
        Items.Remove(item);
    }

    private static Monkey GetMonkeyWithId(Monkey[] monkeys, int toMonkeyId)
    {
        return monkeys.Single(m => m.Id == toMonkeyId);
    }
}

public class Item
{
    public Item(int worryLevel)
    {
        WorryLevel = worryLevel;
    }

    public int WorryLevel { get; set; }
};