using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day13;

public static class Day13Puzzle
{
    // Returns sum of 1-based indices
    public static int GetSumOfIndicesOfOrderedPairs(PacketPair[] packetPairs)
    {
        var orderedPairIndices = new List<int>();
        for (int idx = 0; idx < packetPairs.Length; idx++)
        {
            if (packetPairs[idx].IsOrdered() == true)
                orderedPairIndices.Add(idx + 1);
        }
        return orderedPairIndices.Sum();
    }
}

public record PacketPair(Packet Left, Packet Right)
{
    public bool? IsOrdered()
    {
        return Left.Items.IsOrdered(Right.Items);
    }
}

public record PacketItemPair(PacketItem? Left, PacketItem? Right)
{
    public bool? IsOrdered()
    {
        return Left == null
            ? Right != null ? true : null
            : Right == null
                ? false
                : Left.IsOrdered(Right);
    }
}

public record Packet(ListPacketItem Items);

public abstract record PacketItem
{
    public abstract bool? IsOrdered(PacketItem right);
}

public record ListPacketItem() : PacketItem
{
    public ListPacketItem(params PacketItem[] value) : this()
    {
        Value = value;
    }

    public PacketItem[] Value { get; }

    public override bool? IsOrdered(PacketItem right)
    {
        return right switch
        {
            IntegerPacketItem integerRight => IsOrdered(integerRight),
            ListPacketItem listRight => IsOrdered(listRight),
            _ => throw new ArgumentOutOfRangeException(nameof(right))
        };
    }

    private bool? IsOrdered(IntegerPacketItem right)
    {
        return IsOrdered(new ListPacketItem(right));
    }

    private bool? IsOrdered(ListPacketItem right)
    {
        var (leftPadded, rightPadded) = PadToMatchLengths(Value, right.Value);
        return leftPadded
            .Zip(rightPadded)
            .Select(pair => new PacketItemPair(pair.First, pair.Second).IsOrdered())
            .FirstOrDefault(result => result != null);
    }

    private (PacketItem?[], PacketItem?[] rightItems) PadToMatchLengths(PacketItem[] leftItems, PacketItem[] rightItems)
    {
        if (leftItems.Length < rightItems.Length)
            return (
                leftItems.Concat(Enumerable.Repeat<PacketItem?>(null, rightItems.Length - leftItems.Length)).ToArray(),
                rightItems
            );

        if (leftItems.Length > rightItems.Length)
            return (
                leftItems,
                rightItems.Concat(Enumerable.Repeat<PacketItem?>(null, leftItems.Length - rightItems.Length)).ToArray()
            );
        return (leftItems, rightItems);
    }
}

public record IntegerPacketItem() : PacketItem
{
    public IntegerPacketItem(int value) : this()
    {
        Value = value;
    }

    public int Value { get; }

    public override bool? IsOrdered(PacketItem right)
    {
        return right switch
        {
            IntegerPacketItem integerRight => IsOrdered(integerRight),
            ListPacketItem listRight => IsOrdered(listRight),
            _ => throw new ArgumentOutOfRangeException(nameof(right))
        };
    }

    private bool? IsOrdered(IntegerPacketItem right)
    {
        if (Value < right.Value)
            return true;
        if (Value > right.Value)
            return false;
        return null;
    }

    private bool? IsOrdered(ListPacketItem right)
    {
        return new ListPacketItem(this).IsOrdered(right);
    }
}