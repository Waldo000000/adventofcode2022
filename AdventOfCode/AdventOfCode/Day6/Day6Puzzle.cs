using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day6;

public static class Day6Puzzle
{
    private const int StartOfPacketMarkerLength = 4;

    public static (int, bool) GetCharactersProcessedBeforeStartOfPacketMarker(StreamReader streamReader)
    {
        var buff = new List<char>();
        var numRead = 0;
        while (!IsStartOfPacket(buff) && !streamReader.EndOfStream)
        {
            buff = AddCharToBuffer(buff, streamReader.Read());
            numRead++;
        }

        return (numRead, IsStartOfPacket(buff));
    }

    private static List<char> AddCharToBuffer(List<char> buff, int ch)
    {
        return buff.Append((char) ch).TakeLast(StartOfPacketMarkerLength).ToList();
    }

    private static bool IsStartOfPacket(List<char> buff)
    {
        return buff.Count == StartOfPacketMarkerLength &&
               buff.Distinct().Count() == StartOfPacketMarkerLength;
    }
}