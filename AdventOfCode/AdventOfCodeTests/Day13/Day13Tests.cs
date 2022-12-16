using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Day13;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCodeTests.Day13;

public class Day13Tests
{
    [Test]
    public void GetSumOfIndicesOfOrderedPairs_WithSampleData_ReturnsExpectedValue()
    {
        var packetPairs = ReadPacketPairs("Day13\\Part1.sample.txt").ToArray();

        Day13Puzzle.GetSumOfIndicesOfOrderedPairs(packetPairs).Should().Be(13);
    }

    [Test]
    public void GetSumOfIndicesOfOrderedPairs_WithRealData_ReturnsExpectedValue()
    {
        var packetPairs = ReadPacketPairs("Day13\\Part1.real.txt").ToArray();

        Day13Puzzle.GetSumOfIndicesOfOrderedPairs(packetPairs).Should().Be(5720);
    }

    [Test]
    public void GetDecoderKey_WithSampleData_ReturnsExpectedValue()
    {
        var packetPairs = ReadPacketPairs("Day13\\Part1.sample.txt").ToArray();

        var packets = packetPairs
            .SelectMany(packetPair => new[] {packetPair.Left, packetPair.Right})
            .ToArray();

        Day13Puzzle.GetDecoderKey(packets.ToArray()).Should().Be(140);
    }

    [Test]
    public void GetDecoderKey_WithRealData_ReturnsExpectedValue()
    {
        var packetPairs = ReadPacketPairs("Day13\\Part1.real.txt").ToArray();

        var packets = packetPairs
            .SelectMany(packetPair => new[] {packetPair.Left, packetPair.Right})
            .ToArray();

        Day13Puzzle.GetDecoderKey(packets.ToArray()).Should().Be(23504);
    }

    private IEnumerable<PacketPair> ReadPacketPairs(string filename)
    {
        return File.ReadAllLines(filename)
            .Chunk(3)
            .Select(chunk =>
            {
                Packet leftPacket = ReadPacket(chunk.First());
                var rightPacket = ReadPacket(chunk.Skip(1).First());
                return new PacketPair(leftPacket, rightPacket);
            }).ToArray();
    }

    private Packet ReadPacket(string line)
    {
        return new Packet(ReadListPacketItem(line));
    }

    private PacketItem ReadPacketItem(string chars)
    {
        return IsList(chars)
            ? ReadListPacketItem(chars)
            : new IntegerPacketItem(int.Parse(chars));
    }

    private ListPacketItem ReadListPacketItem(string chars)
    {
        var unwrapped = string.Concat(chars.Skip(1).SkipLast(1));

        var innerPacketItems = Split(unwrapped)
            .Select(ReadPacketItem)
            .ToArray();

        return new ListPacketItem(innerPacketItems);
    }

    private static IEnumerable<string> Split(string unwrapped)
    {
        var nesting = 0;
        IList<char> chars = new List<char>();
        foreach (var ch in unwrapped)
        {
            if (ch == '[')
                nesting++;

            if (ch == ']')
                nesting--;

            if (nesting == 0)
            {
                if (ch == ',')
                {
                    yield return string.Concat(chars);
                    chars.Clear();
                }
                else
                {
                    chars.Add(ch);
                }
            }
            else
            {
                chars.Add(ch);
            }
        }

        if (chars.Any())
            yield return string.Concat(chars);
    }

    private bool IsList(string chars)
    {
        return chars.First() == '[';
    }
}