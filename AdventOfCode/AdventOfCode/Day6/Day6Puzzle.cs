using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day6;

public static class Day6Puzzle
{
    private const int StartOfPacketMarkerLength = 4;
    private const int StartOfMessageMarkerLength = 14;

    public static (int, bool) GetCharactersProcessedBeforeStartOfPacketMarker(StreamReader streamReader)
    {
        return ReadUntilMarker(streamReader, StartOfPacketMarkerLength);
    }

    public static (int, bool) GetCharactersProcessedBeforeStartOfMessageMarker(StreamReader streamReader)
    {
        return ReadUntilMarker(streamReader, StartOfMessageMarkerLength);
    }

    private static (int, bool) ReadUntilMarker(StreamReader streamReader, int bufferMaxLength)
    {
        var buff = new List<char>();
        var numRead = 0;
        
        while (!IsUniqueCharsMarkerDetected(buff, bufferMaxLength) && !streamReader.EndOfStream)
        {
            buff = AddCharToBuffer(buff, streamReader.Read(), bufferMaxLength);
            numRead++;
        }

        return (numRead, IsUniqueCharsMarkerDetected(buff, bufferMaxLength));
    }

    private static List<char> AddCharToBuffer(List<char> buff, int ch, int bufferMaxLength)
    {
        return buff.Append((char) ch).TakeLast(bufferMaxLength).ToList();
    }

    private static bool IsUniqueCharsMarkerDetected(List<char> buff, int markerLength)
    {
        return buff.Count == markerLength &&
               buff.Distinct().Count() == markerLength;
    }
}