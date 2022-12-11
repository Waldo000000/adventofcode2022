using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day10;

public static class Day10Puzzle
{
    public static IEnumerable<ProcessorState> GetState(Operation[] operations)
    {
        var processor = new Processor(0, 1);
        return operations.ToList().SelectMany(processor.Process);
    }

    public static int GetSumOfSignalStrengths(Operation[] operations, int firstMeasurementCycle,
        int measurementCycleInterval)
    {
        var processor = new Processor(0, 1);
        var processorStates = operations.ToList().SelectMany(processor.Process).ToList();
        return processorStates
            .Where((_, cycle) => IsInterestingSignal(firstMeasurementCycle, measurementCycleInterval, cycle + 1))
            .Sum(state => state.SignalStrength);
    }

    private static bool IsInterestingSignal(int firstMeasurementCycle, int measurementCycleInterval, int cycleNumber)
    {
        return cycleNumber >= firstMeasurementCycle &&
               (cycleNumber - firstMeasurementCycle) % measurementCycleInterval == 0;
    }
}

public record ProcessorState(int SignalStrength, int RegisterXValue);

public class Processor
{
    public int Cycle { get; private set; }
    public int RegisterXValue { get; private set; }

    public int SignalStrength => Cycle * RegisterXValue;

    public Processor(int cycle, int registerXValue)
    {
        Cycle = cycle;
        RegisterXValue = registerXValue;
    }

    public IEnumerable<ProcessorState> Process(Operation operation)
    {
        switch (operation)
        {
            case NoOperation:
                return ProcessNoOperation();
            case AddOperation addOperation:
                return ProcessAddOperation(addOperation);
            default:
                throw new ArgumentOutOfRangeException($"Unrecognized operation");
        }
    }

    private IEnumerable<ProcessorState> ProcessNoOperation()
    {
        yield return Tick();
    }

    private IEnumerable<ProcessorState> ProcessAddOperation(AddOperation addOperation)
    {
        yield return Tick();
        yield return Tick();
        RegisterXValue += addOperation.Amount;
    }

    private ProcessorState State() => new(SignalStrength, RegisterXValue);

    private ProcessorState Tick()
    {
        Cycle++;
        return State();
    }
}

public class Operation
{
}

public class AddOperation : Operation
{
    public int Amount { get; }

    public AddOperation(int amount)
    {
        Amount = amount;
    }
}

public class NoOperation : Operation
{
}