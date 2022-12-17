using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Shared;

public static class IntegerExtensions
{
    /// <summary>
    /// Inclusive range of integers between from and to 
    /// </summary>
    public static IEnumerable<int> RangeUntil(this int from, int to)
    {
        var ordered = new[] {from, to}.OrderBy(i => i).ToArray();
        return Enumerable.Range(ordered[0], ordered[1] - ordered[0] + 1);
    }
}