using System;
using System.Linq;

namespace AdventOfCode.Day3;

public static class Day3Puzzle
{
    private const string ItemChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static int GetSumOfPackingErrorPriorities(Rucksack[] rucksacks)
    {
        return rucksacks.Sum(GetPackingErrorPriority);
    }

    private static int GetPackingErrorPriority(Rucksack rucksack)
    {
        return GetItemPriority(GetPackingErrorItem(rucksack));
    }

    private static string GetPackingErrorItem(Rucksack rucksack)
    {
        var allNames = rucksack.Compartments.SelectMany(c => c.Items).Select(item => item.Name).Distinct();
        var errorItems = allNames.Where(name => rucksack.Compartments.Count(c => c.Items.Any(i => i.Name == name)) > 1);
        return errorItems.Single();
    }

    private static int GetItemPriority(string item)
    {
        return ItemChars.IndexOf(item, StringComparison.Ordinal) + 1;
    }
}

public record Rucksack(Compartment[] Compartments);

public record Compartment(Item[] Items);

public record Item(string Name);