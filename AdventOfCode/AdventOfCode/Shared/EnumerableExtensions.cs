using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Shared;

public static class EnumerableExtensions
{
    public static int Product(this IEnumerable<int> factors)
    {
        return factors.Aggregate(1, (p, c) => p * c);
    }

    public static long Product(this IEnumerable<long> factors)
    {
        return factors.Aggregate(1L, (p, c) => p * c);
    }
}