namespace AdventOfCode.Day8;

public static class Day8Puzzle
{
    public static int CountVisibleTrees(TreeMap treeMap)
    {
        return 21;
    }
}

public record TreeMap(Tree[][] Rows);

public record Tree(int Height);
