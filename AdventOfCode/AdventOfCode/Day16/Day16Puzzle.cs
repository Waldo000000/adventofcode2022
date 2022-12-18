using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day16;

public static class Day16Puzzle
{
    /// <summary>
    /// (This number specifies how many of the most promising tokens for each node to retain when pruning at each time
    /// step iteration. I don't know why 1 isn't enough... but I hit my timebox for this day's challenge, sorry;
    /// lucky it's just for fun! :-D)
    /// </summary>
    private static readonly int MagicNumber = 10;

    public static int GetMaxPressureReleaseWithinMinutes(int minutes, string initialValveId,
        Dictionary<string, Valve> valves, int numActors)
    {
        var tokens = new[] {Token.CreateInitial(valves[initialValveId], numActors)};
        for (int minutesLeft = minutes; minutesLeft > 0; minutesLeft--)
        {
            tokens = GetNewTokens(tokens, valves, minutesLeft, numActors);
        }

        return tokens
            .OrderByDescending(t => t.TotalForecastPressureReleased)
            .Max(t => t.TotalForecastPressureReleased);
    }

    private static Token[] GetNewTokens(Token[] tokens, Dictionary<string, Valve> valves, int minutesLeft,
        int numActors)
    {
        var allNewTokens = tokens.SelectMany(token => GetNewTokens(valves, minutesLeft, token, numActors));
        var prunedNewTokens = allNewTokens
            .GroupBy(GetTokenKey)
            .SelectMany(g => g.OrderByDescending(t => t.TotalForecastPressureReleased).Take(MagicNumber));
        return prunedNewTokens.ToArray();
    }

    /// <summary>
    /// Key to uniquely describe set of current actor locations   
    /// </summary>
    private static string GetTokenKey(Token token)
    {
        return string.Join("", token.Valves.Select(v => v.Id));
    }

    /// <summary>
    /// Have all actors take some combination of actions
    /// </summary>
    private static IEnumerable<Token> GetNewTokens(Dictionary<string, Valve> valves, int minutesLeft, Token token, int numActors)
    {
        return Enumerable
            .Range(0, numActors)
            .Aggregate(
                new[] {token},
                (tokensAfterPreviousActorsHaveActed, actorIdx) => tokensAfterPreviousActorsHaveActed
                    .SelectMany(t => EnumeratePossibleActions(t, valves, minutesLeft, actorIdx))
                    .ToArray()
            );
    }

    /// <summary>
    /// Either go to another valve, or open the valve at hand
    /// </summary>
    private static IEnumerable<Token> EnumeratePossibleActions(Token token, Dictionary<string, Valve> valves, int minutesLeft, int actorIdx)
    {
        return token.Valves[actorIdx].ToValveIds
            .Select(toValveId => token.GoTo(valves[toValveId], actorIdx))
            .Append(token.OpenValve(minutesLeft, actorIdx));
    }
}

public record Valve(string Id, int FlowRate, string[] ToValveIds);

public record Token(Valve[] Valves, string[] OpenedValveIds, int TotalForecastPressureReleased, Token[] History)
{
    public static Token CreateInitial(Valve valve, int numActors)
    {
        return new Token(
            Enumerable.Repeat(valve, numActors).ToArray(),
            Array.Empty<string>(),
            0,
            Array.Empty<Token>()
        );
    }

    public Token GoTo(Valve valve, int actorIdx)
    {
        var valves = (Valve[]) Valves.Clone();
        valves[actorIdx] = valve;
        return this with
        {
            Valves = valves,
            History = new[] {this}.Concat(History).ToArray()
        };
    }

    public Token OpenValve(int minutesLeft, int actorIdx)
    {
        var valve = Valves[actorIdx];
        return this with
        {
            TotalForecastPressureReleased = TotalForecastPressureReleased +
                                            (OpenedValveIds.Contains(valve.Id)
                                                ? 0
                                                : valve.FlowRate * (minutesLeft - 1)),
            OpenedValveIds = OpenedValveIds.Append(valve.Id).ToArray(),
            History = new[] {this}.Concat(History).ToArray()
        };
    }
}