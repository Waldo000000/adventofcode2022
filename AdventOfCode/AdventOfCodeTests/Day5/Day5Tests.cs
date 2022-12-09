using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Day5;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day5;

public class Day5Tests
{
    private static readonly Regex CrateRegex = new Regex(@"\[(\w*)\]\s*");
    private static readonly Regex CrateIdLineRegex = new Regex(@"^[\s\d]+$");
    private static readonly Regex MovementOperationRegex = new Regex(@"^move (\d*) from (\d*) to (\d*)$");

    [Test]
    public void GetStackTopsAfterRearrangement_WithSampleData_ReturnsExpectedResult()
    {
        var (stacks, rearrangementProcedure) = ReadRearrangementProcedure("Day5\\Part1.sample.txt");
        Day5Puzzle.RearrangeStacks(stacks, rearrangementProcedure);
        Day5Puzzle.GetStackTopSummary(stacks).Should().Be("CMZ");
    }
    
    [Test]
    public void GetStackTopsAfterRearrangement_WithRealData_ReturnsExpectedResult()
    {
        var (stacks, rearrangementProcedure) = ReadRearrangementProcedure("Day5\\Part1.real.txt");
        Day5Puzzle.RearrangeStacks(stacks, rearrangementProcedure);
        Day5Puzzle.GetStackTopSummary(stacks).Should().Be("ZSQVCCJLL");
    }

    private static (Stack<string>[], RearrangementProcedure) ReadRearrangementProcedure(string inputFilepath)
    {
        var lines = File.ReadAllLines(inputFilepath).ToList();
        var stacks = ParseStackContents(lines);
        var movementOperations = ParseMovementOperations(lines);

        return (stacks.ToArray(), new RearrangementProcedure(movementOperations));
    }

    private static List<Stack<string>> ParseStackContents(IReadOnlyList<string> lines)
    {
        var stacks = lines
            .Single(line => CrateIdLineRegex.IsMatch(line))
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(stackId => new Stack<string>())
            .ToList();

        foreach (var line in lines.Where(line => CrateRegex.IsMatch(line)).Reverse())
        {
            var cratesAtThisLevelForEachStack = line.ToCharArray().Chunk(4).Select(ch => new string(ch)).ToList();
            cratesAtThisLevelForEachStack.Zip(stacks).ToList().ForEach(tuple =>
            {
                var (crateAtThisLevel, stack) = tuple;
                var match = CrateRegex.Match(crateAtThisLevel);
                if (match.Success)
                {
                    stack.Push(match.Groups[1].ToString());
                }
            });
        }

        return stacks;
    }

    private static MovementOperation[] ParseMovementOperations(IReadOnlyList<string> lines)
    {
        return lines
            .Select(line => MovementOperationRegex.Match(line))
            .Where(match => match.Success)
            .Select(match => new MovementOperation(
                int.Parse(match.Groups[1].ToString()),
                int.Parse(match.Groups[2].ToString()),
                int.Parse(match.Groups[3].ToString())
            )).ToArray();
    }
}