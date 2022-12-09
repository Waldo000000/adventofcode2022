using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace AdventOfCode.Day8;

public static class Day8Puzzle
{
    public static int GetMaxScenicScore(TreeMap treeMap)
    {
        return GetScenicScores(treeMap).Max(row => row.Max());
    }

    private static int[][] GetScenicScores(TreeMap treeMap)
    {
        var numRows = treeMap.Rows.Length;
        var numCols = treeMap.Rows.First().Length;
        int[][] result = new int[numRows][];

        for (int rowIdx = 0; rowIdx < numRows; rowIdx++)
        {
            result[rowIdx] = new int[numCols];

            for (int colIdx = 0; colIdx < numCols; colIdx++)
            {
                result[rowIdx][colIdx] = GetScenicScore(treeMap, rowIdx, colIdx);
            }
        }

        return result;
    }

    private static int GetScenicScore(TreeMap treeMap, int rowIdx, int colIdx)
    {
        var maxRowIdx = treeMap.Rows.Length - 1;
        var maxColIdx = treeMap.Rows.First().Length - 1;
        var scenicScoreLookingRight = GetScenicScoreLookingRight(treeMap.Rows, rowIdx, colIdx);
        var scenicScoreLookingLeft = GetScenicScoreLookingRight(treeMap.Rows.Select(row => row.Reverse().ToArray()).ToArray(), rowIdx, maxColIdx - colIdx);
        
        var scenicScoreLookingDown = GetScenicScoreLookingRight(treeMap.Rows.Transpose(), colIdx, rowIdx);
        var scenicScoreLookingUp = GetScenicScoreLookingRight(treeMap.Rows.Transpose().Select(row => row.Reverse().ToArray()).ToArray(), colIdx, maxRowIdx - rowIdx);

        return scenicScoreLookingRight * scenicScoreLookingLeft * scenicScoreLookingDown * scenicScoreLookingUp;
    }

    private static int GetScenicScoreLookingRight(Tree[][] treeMap, int rowIdx, int colIdx)
    {
        var treeHeight = treeMap[rowIdx][colIdx];
        var scenicScoreLookingRight = treeMap[rowIdx].Skip(colIdx + 1).TakeWhile(col => treeHeight.Height > col.Height).Count();
        var numCols = treeMap.First().Length;
        return colIdx + scenicScoreLookingRight < numCols - 1
            ? scenicScoreLookingRight + 1 // add the tree that was at equal height
            : scenicScoreLookingRight;
    }

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
                 result[rowIdx, colIdx] = masks.Select(mask => mask[rowIdx][colIdx]).Any(v => v);
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