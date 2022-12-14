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
        var packetPairs = ReadPacketPairs("Day12\\Part1.sample.txt").ToArray();

        Day13Puzzle.GetSumOfIndicesOfOrderedPairs(packetPairs).Should().Be(13);
    }

    private IEnumerable<PacketPair> ReadPacketPairs(string filename)
    {
        yield return new PacketPair(
            new Packet(new ListPacketItem(
                new IntegerPacketItem(1),
                new IntegerPacketItem(1),
                new IntegerPacketItem(3),
                new IntegerPacketItem(1),
                new IntegerPacketItem(1))
            ),
            new Packet(new ListPacketItem(
                new IntegerPacketItem(1),
                new IntegerPacketItem(1),
                new IntegerPacketItem(5),
                new IntegerPacketItem(1),
                new IntegerPacketItem(1)
            )));
        yield return new PacketPair(
            new Packet(new ListPacketItem(
                new ListPacketItem(new IntegerPacketItem(1)),
                new ListPacketItem(new IntegerPacketItem(2), new IntegerPacketItem(3), new IntegerPacketItem(4))
            )),
            new Packet(new ListPacketItem(
                new ListPacketItem(new IntegerPacketItem(1)),
                new IntegerPacketItem(4)
            )));

        
        // TODO
        // return File.ReadAllLines(filename)
        //     .Chunk(3)
        //     .Select(chunk =>
        //     {
        //         var packetPairLines = chunk.Take(2).ToArray();
        //         Packet leftPacket = ReadPacket(chunk.First());
        //         var rightPacket = ReadPacket(chunk.Skip(1).First());
        //         return new PacketPair(leftPacket, rightPacket);
        //     }).ToArray();
    }

    private Packet ReadPacket(string line)
    {
        throw new System.NotImplementedException();
    }
}