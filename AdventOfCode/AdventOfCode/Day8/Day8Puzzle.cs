using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day8;

public static class Day8Puzzle
{
    public static int CountVisibleTrees(TreeMap treeMap)
    {
        var visibilityFromLeft = treeMap.Rows.Select(GetVisibilityFromLeft).ToArray();
        var visibilityFromRight = treeMap.Rows.Select(row => row.Reverse()).Select(GetVisibilityFromLeft).Select(row => row.Reverse().ToArray()).ToArray();
        var visibilityFromTop = treeMap.Rows.Transpose().Select(GetVisibilityFromLeft).ToArray().Transpose();
        var visibilityFromBottom = treeMap.Rows.Transpose().Select(row => row.Reverse()).Select(GetVisibilityFromLeft).Select(row => row.Reverse().ToArray()).ToArray().Transpose();

        var visibility = BooleanOr(visibilityFromLeft, visibilityFromRight, visibilityFromTop, visibilityFromBottom);

        return FlattenMultidimensionalArray(visibility).Sum(Convert.ToInt32);
    }

    private static bool[,] BooleanOr(params bool[][][] masks)
    {
        var numRows = masks.First().Length;
        var numCols = masks.First().First().Length;
        var result = new bool[numRows, numCols];

        for (int rowIdx = 0; rowIdx < numRows; rowIdx++)
        {
            for (int colIdx = 0; colIdx < numCols; colIdx++)
            {
                 result[rowIdx, colIdx] = masks.Select(mask => mask[rowIdx][colIdx]).Any(v => v == true);
            }
        }

        return result;
    }

    private static IEnumerable<bool> FlattenMultidimensionalArray(bool[,] visibility)
    {
        return visibility.Cast<bool>();
    }


    private static bool[] GetVisibilityFromLeft(IEnumerable<Tree> row)
    {
        var maxSoFar = -1;
        var visibility = new List<bool>();
        foreach (var tree in row)
        {
            visibility.Add(tree.Height > maxSoFar);
            maxSoFar = Math.Max(maxSoFar, tree.Height);
        }

        return visibility.ToArray();
    }
}

public record TreeMap(Tree[][] Rows);

public record Tree(int Height);