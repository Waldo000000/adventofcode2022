using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day5;

public static class Day5Puzzle
{
    public static void RearrangeStacks(Stack<string>[] stacks, RearrangementProcedure rearrangementProcedure)
    {
        foreach (var movementOperation in rearrangementProcedure.MovementOperations)
        {
            Enumerable.Range(0, movementOperation.NumCrates).ToList().ForEach(_ =>
            {
                var popped = stacks[movementOperation.FromStack-1].Pop();
                stacks[movementOperation.ToStack-1].Push(popped);
            });
        }
    }

    public static string GetStackTopSummary(Stack<string>[] stacks)
    {
        return string.Join("", stacks.Select(stack => stack.Peek()));
    }
}
public record RearrangementProcedure(MovementOperation[] MovementOperations);

public record MovementOperation(int NumCrates, int FromStack, int ToStack);