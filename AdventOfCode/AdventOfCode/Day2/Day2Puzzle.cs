using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day2;

public static class Day2Puzzle
{
    private static readonly Dictionary<string, HandShape> StrategyCipherForPlayerHandShape = new()
    {
        {"X", HandShape.Rock},
        {"Y", HandShape.Paper},
        {"Z", HandShape.Scissors},
    };

    private static readonly Dictionary<string, HandShape> StrategyCipherForOpponentHandShape = new()
    {
        {"A", HandShape.Rock},
        {"B", HandShape.Paper},
        {"C", HandShape.Scissors}
    };

    private static readonly Dictionary<string, Outcome> StrategyCipherForPlayerOutcome = new()
    {
        {"X", Outcome.Loss},
        {"Y", Outcome.Draw},
        {"Z", Outcome.Win},
    };

    private static readonly Dictionary<HandShape, int> HandShapeScore = new()
    {
        {HandShape.Rock, 1},
        {HandShape.Paper, 2},
        {HandShape.Scissors, 3}
    };

    private static readonly Dictionary<Outcome, int> OutcomeScore = new()
    {
        {Outcome.Loss, 0},
        {Outcome.Draw, 3},
        {Outcome.Win, 6}
    };

    private static readonly Dictionary<HandShape, Dictionary<HandShape, Outcome>> PlayerVsOpponentOutcomes = new()
    {
        {
            HandShape.Rock,
            new() {{HandShape.Rock, Outcome.Draw}, {HandShape.Paper, Outcome.Loss}, {HandShape.Scissors, Outcome.Win}}
        },
        {
            HandShape.Paper,
            new() {{HandShape.Paper, Outcome.Draw}, {HandShape.Scissors, Outcome.Loss}, {HandShape.Rock, Outcome.Win}}
        },
        {
            HandShape.Scissors,
            new() {{HandShape.Scissors, Outcome.Draw}, {HandShape.Rock, Outcome.Loss}, {HandShape.Paper, Outcome.Win}}
        },
    };

    private static Outcome GetOutcome(Turn turn)
    {
        return PlayerVsOpponentOutcomes[turn.PlayerTurn][turn.OpponentTurn];
    }

    public static int GetTotalScore(StrategyGuide strategyGuide)
    {
        return strategyGuide.Turns.Sum(turn => OutcomeScore[GetOutcome(turn)] + HandShapeScore[turn.PlayerTurn]);
    }

    public static StrategyGuide DecryptStrategyGuideFromHandShapeGuide(IEnumerable<string> lines)
    {
        var turns = lines.Select(line =>
        {
            var tokens = line.Split(" ");
            return new Turn(StrategyCipherForOpponentHandShape[tokens[0]], StrategyCipherForPlayerHandShape[tokens[1]]);
        }).ToList();
        return new StrategyGuide(turns);
    }

    public static StrategyGuide DecryptStrategyGuideFromOutcomeGuide(IEnumerable<string> lines)
    {
        var turns = lines.Select(line =>
        {
            var tokens = line.Split(" ");
            var opponentTurn = StrategyCipherForOpponentHandShape[tokens[0]];
            var playerOutcome = StrategyCipherForPlayerOutcome[tokens[1]];
            var playerTurn = PlayerVsOpponentOutcomes
                .SelectMany(playerTurnMap => playerTurnMap.Value.Where(opponentTurnMap =>
                    opponentTurnMap.Key == opponentTurn && opponentTurnMap.Value == playerOutcome).Select(_ => playerTurnMap.Key)).Single();

            return new Turn(opponentTurn, playerTurn);
        }).ToList();
        return new StrategyGuide(turns);
    }
}

public record StrategyGuide(List<Turn> Turns);

public record Turn(HandShape OpponentTurn, HandShape PlayerTurn);

public enum HandShape
{
    Rock,
    Paper,
    Scissors
}

public enum Outcome
{
    Loss,
    Draw,
    Win
}