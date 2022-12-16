using System;
using System.Diagnostics;
using System.Linq;
using AdventOfCode.Shared;

namespace AdventOfCode.Day13;

public static class Day13Puzzle
{
    public static readonly Packet[] DividerPackets =
    {
        new Packet(new ListPacketItem(new ListPacketItem(new IntegerPacketItem(2)))),
        new Packet(new ListPacketItem(new ListPacketItem(new IntegerPacketItem(6)))),
    };

    // Returns sum of 1-based indices
    public static int GetSumOfIndicesOfOrderedPairs(PacketPair[] packetPairs)
    {
        return packetPairs
            .Select((pair, idx) => (pair, idx))
            .Where(tuple => tuple.pair.IsInCorrectOrder())
            .Select(tuple => tuple.idx + 1)
            .Sum();
    }

    public static int GetDecoderKey(Packet[] packets)
    {
        var packetsWithDividers = packets
            .Concat(DividerPackets)
            .ToList();

        packetsWithDividers.Sort();

        return DividerPackets
            .Select(dividerPacket => packetsWithDividers.IndexOf(dividerPacket) + 1)
            .Product();
    }
}

public record PacketPair(Packet Left, Packet Right)
{
    public bool IsInCorrectOrder() => Left.CompareTo(Right) < 0;

    public int CompareTo()
    {
        return Left.Items.CompareTo(Right.Items);
    }
}

public record Packet(ListPacketItem Items) : IComparable<Packet>
{
    public int CompareTo(Packet? other)
    {
        return Items.CompareTo(other.Items);
    }

    public override string ToString()
    {
        return Items.ToString();
    }
}

public abstract record PacketItem
{
    public abstract int CompareTo(PacketItem right);
}

public record ListPacketItem() : PacketItem
{
    public ListPacketItem(params PacketItem[] value) : this()
    {
        Value = value;
    }

    public PacketItem[] Value { get; }

    public override int CompareTo(PacketItem right)
    {
        return right switch
        {
            IntegerPacketItem integerRight => CompareTo(integerRight),
            ListPacketItem listRight => CompareTo(listRight),
            _ => throw new ArgumentOutOfRangeException(nameof(right))
        };
    }

    private int CompareTo(IntegerPacketItem right)
    {
        return CompareTo(new ListPacketItem(right));
    }

    private int CompareTo(ListPacketItem right)
    {
        var comparisons = Value
            .Zip(right.Value)
            .Select(pair => pair.First.CompareTo(pair.Second));

        var firstNonEqualComparison = comparisons.FirstOrDefault(c => c != 0);
        if (firstNonEqualComparison != 0) return firstNonEqualComparison;
        return Value.Length.CompareTo(right.Value.Length);
    }

    public override string ToString()
    {
        return string.Concat("[", string.Join(',', (object[])Value), "]");
    }
}

[DebuggerDisplay("{Value}")]
public record IntegerPacketItem() : PacketItem
{
    public IntegerPacketItem(int value) : this()
    {
        Value = value;
    }

    public int Value { get; }

    public override int CompareTo(PacketItem right)
    {
        return right switch
        {
            IntegerPacketItem integerRight => IsOrdered(integerRight),
            ListPacketItem listRight => IsOrdered(listRight),
            _ => throw new ArgumentOutOfRangeException(nameof(right))
        };
    }

    private int IsOrdered(IntegerPacketItem right)
    {
        return Value.CompareTo(right.Value);
    }

    private int IsOrdered(ListPacketItem right)
    {
        return new ListPacketItem(this).CompareTo(right);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}