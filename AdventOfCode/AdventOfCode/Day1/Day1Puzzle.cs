using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day1;

public static class Day1Puzzle
{
    public static int GetMaxCaloriesFromTopElves(IReadOnlyList<Elf> elves)
    {
        var elfWithMaxCalories = elves.MaxBy(elf => elf.GetTotalCalories());
        if (elfWithMaxCalories is null) throw new Exception("No elf found with the most calories");
        return elfWithMaxCalories.GetTotalCalories();
    }

    public static int GetMaxCaloriesFromTopNElves(IReadOnlyList<Elf> elves, int n)
    {
        var enumerable = elves.OrderByDescending(elf => elf.GetTotalCalories()).Take(n);
        return enumerable.Sum(elf => elf.GetTotalCalories());
    }
}

public record Elf(List<Food> Food)
{
    public int GetTotalCalories() => Food.Sum(f => f.Calories);
};

public record Food(int Calories);
