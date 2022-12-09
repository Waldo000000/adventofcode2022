using System;
using System.Linq;

namespace AdventOfCode.Day3;

public static class Day3Puzzle
{
    private const string ItemNames = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static Item[] AllItems => ItemNames.Select(n => new Item(n.ToString())).ToArray();

    private static int GetItemPriority(Item item)
    {
        return ItemNames.IndexOf(item.Name, StringComparison.Ordinal) + 1;
    }

    public static int GetSumOfPackingErrorPriorities(Rucksack[] rucksacks)
    {
        return rucksacks.Sum(GetPackingErrorPriority);
    }

    private static int GetPackingErrorPriority(Rucksack rucksack)
    {
        return GetItemPriority(GetPackingErrorItem(rucksack));
    }

    private static Item GetPackingErrorItem(Rucksack rucksack)
    {
        var errorItems = AllItems.Where(item => rucksack.Compartments.Count(c => c.Items.Contains(item)) > 1);
        return errorItems.Single();
    }

    public static int GetSumOfBadgePriorities(Rucksack[] rucksacks)
    {
        return rucksacks.Chunk(3)
            .Select(GetBadge)
            .Sum(GetItemPriority);
    }

    private static Item GetBadge(Rucksack[] group)
    {
        return AllItems.Single(item => group.All(g => g.Items.Contains(item)));
    }
}

public record Rucksack(Compartment[] Compartments)
{
    public readonly Item[] Items = Compartments.SelectMany(c => c.Items).ToArray();
};

public record Compartment(Item[] Items);

public record Item(string Name);