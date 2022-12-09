using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day1;

public static class Day1Puzzle
{
    public static int GetMaxCaloriesFromOneElf(IReadOnlyList<Elf> elves)
    {
        var elfWithMaxCalories = elves.MaxBy(elf => elf.GetTotalCalories());
        if (elfWithMaxCalories is null) throw new Exception("No elf found with the most calories");
        return elfWithMaxCalories.GetTotalCalories();
    }
}

public record Elf(List<Food> Food)
{
    public int GetTotalCalories() => Food.Sum(f => f.Calories);
};

public record Food(int Calories);
