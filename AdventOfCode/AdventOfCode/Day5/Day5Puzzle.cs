using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day5;

public static class Day5Puzzle
{
    public static void RearrangeStacksWithCrateMover9000(Stack<string>[] stacks, RearrangementProcedure rearrangementProcedure)
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

    public static void RearrangeStacksWithCrateMover9001(Stack<string>[] stacks, RearrangementProcedure rearrangementProcedure)
    {
        foreach (var movementOperation in rearrangementProcedure.MovementOperations)
        {
            var popped = Enumerable.Range(0, movementOperation.NumCrates).Select(_ => stacks[movementOperation.FromStack - 1].Pop());
            popped.Reverse().ToList().ForEach(crate => stacks[movementOperation.ToStack - 1].Push(crate));
        }
    }

    public static string GetStackTopSummary(Stack<string>[] stacks)
    {
        return string.Join("", stacks.Select(stack => stack.Peek()));
    }
}
public record RearrangementProcedure(MovementOperation[] MovementOperations);

public record MovementOperation(int NumCrates, int FromStack, int ToStack);