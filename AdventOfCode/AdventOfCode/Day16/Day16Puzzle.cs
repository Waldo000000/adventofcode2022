using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day16;

public static class Day16Puzzle
{
    /// <summary>
    /// (This number specifies how many of the most promising tokens for each node to retain when pruning at each time
    /// step iteration. I don't know why 1 isn't enough... but I hit my timebox for this days' challenge, sorry;
    /// lucky it's just for fun! :-D)
    /// </summary>
    private static readonly int MagicNumber = 8;

    public static int GetMaxPressureReleaseWithinMinutes(int minutes, string initialValveId, Dictionary<string, Valve> valves)
    {
        var tokens = new[] {Token.CreateInitial(valves[initialValveId])};
        for (int minutesLeft = minutes; minutesLeft > 0; minutesLeft--)
        {
            tokens = GetNewTokens(tokens, valves, minutesLeft);
        }

        return tokens
            .OrderByDescending(t => t.TotalForecastPressureReleased)
            .Max(t => t.TotalForecastPressureReleased);
    }

    private static Token[] GetNewTokens(Token[] tokens, Dictionary<string, Valve> valves, int minutesLeft)
    {
        var allNewTokens = tokens.SelectMany(token => GetNewTokens(valves, minutesLeft, token));
        var prunedNewTokens = allNewTokens
            .GroupBy(t => t.Valve)
            .SelectMany(g => g.OrderByDescending(t => t.TotalForecastPressureReleased).Take(MagicNumber));
        return prunedNewTokens.ToArray();
    }

    private static IEnumerable<Token> GetNewTokens(Dictionary<string, Valve> valves, int minutesLeft, Token token)
    {
        return token.Valve.ToValveIds
            .Select(toValveId => token.GoTo(valves[toValveId]))
            .Append(token.OpenValve(minutesLeft));
    }
}

public record Valve(string Id, int FlowRate, string[] ToValveIds);

public record Token(Valve Valve, string[] OpenedValveIds, int TotalForecastPressureReleased, Token[] History)
{
    public static Token CreateInitial(Valve valve)
    {
        return new Token(valve, Array.Empty<string>(), 0, Array.Empty<Token>());
    }

    public Token GoTo(Valve valve)
    {
        return this with
        {
            Valve = valve,
            History = new[] {this}.Concat(History).ToArray()
        };
    }

    public Token OpenValve(int minutesLeft)
    {
        return this with
        {
            TotalForecastPressureReleased = TotalForecastPressureReleased +
                                            (OpenedValveIds.Contains(Valve.Id)
                                                ? 0
                                                : Valve.FlowRate * (minutesLeft - 1)),
            OpenedValveIds = OpenedValveIds.Append(Valve.Id).ToArray(),
            History = new[] {this}.Concat(History).ToArray()
        };
    }
}