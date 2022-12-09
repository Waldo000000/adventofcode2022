using System.Linq;

namespace AdventOfCode.Day8;

public static class MatrixExtensions
{
    public static T[][] Transpose<T>(this T[][] matrix)
    {
        var colIdxs = Enumerable.Range(0, matrix.First().Length);
        var rows = colIdxs.Select(colIdx => matrix.Select(row => row[colIdx]).ToArray()).ToArray();
        return rows;
    }
}