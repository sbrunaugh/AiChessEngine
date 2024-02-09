using DecisionEngine.Helpers;
using DecisionEngine.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DecisionEngine;

public class Program
{
    static void Main(string[] args)
    {
        if (args == null || args.Length != 2)
            throw new ArgumentException(nameof(args));

        // inputs should have a position and whose turn it is
        var positionInput = args[0];
        positionInput.Replace("\n", "").Replace("\r\n", "").Replace(" ", "");
        var positionStrings = positionInput.Split(',');
        if (positionStrings.Length != 64)
            throw new ArgumentException(nameof(args));
        var currentPosition = BoardHelper.GenerateFreshBoard();

        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                currentPosition[i, j] = int.Parse(positionStrings[(i * 8) + j]);
            }
        }

        Player player;
        if (int.TryParse(args[1], out var playerInt))
        {
            player = playerInt == 1 ? Player.White : Player.Black;
        }
        else
        {
            throw new ArgumentException(nameof(args));
        }

        var legalMoves = FindAllLegalMoves(currentPosition, player, true);

        var preCalculations = new List<FutureCalculation>();
        var enemy = player == Player.White ? Player.Black : Player.White;
        foreach(var move in legalMoves)
        {
            var x = new FutureCalculation()
            {
                Move = move,
                FutureEvaluatedMoves = new List<EvaluatedMove>()
            };

            var secondLayer = FindAllLegalMoves(x.Move.NewPosition.ToIntMatrix(), enemy, true);
            foreach(var nextMove in secondLayer)
            {
                var y = new EvaluatedMove()
                {
                    Move = nextMove,
                    Evaluation = 0f
                };
                x.FutureEvaluatedMoves.Add(y);
            }

            preCalculations.Add(x);
        }

        var outputText = InvokeNeuralNetwork(preCalculations);
        var computedCalculations = JsonSerializer.Deserialize<List<FutureCalculation>>(outputText);

        AnalyzeCalculations(computedCalculations);

        // best option for white is the move that results in a set of moves for black where the lowest of
        // that set is scored as high as possible. Highest of the lowest possibilities.
        var bestMove = player == Player.White
            ? computedCalculations.OrderByDescending(cc => cc.MinAndMaxEvals().Item1).First().Move
            : computedCalculations.OrderBy(cc => cc.MinAndMaxEvals().Item2).First().Move;

        Console.WriteLine(bestMove.MoveName);
        Console.WriteLine(SerializePosition(bestMove, true));
    }

    public static List<Move> FindAllLegalMoves(int[,] board, Player player, bool includeCheckCheck = false)
    {
        var result = new List<Move>();

        result.AddRange(PawnHelper.FindAllLegalMoves(board, player));
        result.AddRange(KnightHelper.FindAllLegalMoves(board, player));
        result.AddRange(BishopHelper.FindAllLegalMoves(board, player));
        result.AddRange(RookHelper.FindAllLegalMoves(board, player));
        result.AddRange(QueenHelper.FindAllLegalMoves(board, player));
        result.AddRange(KingHelper.FindAllLegalMoves(board, player));
        result.AddRange(MoveHelper.FindAllLegalCastles(board, player));

        if(includeCheckCheck)
            MoveHelper.FilterOutMovesResultingInCheck(result, player);

        // TODO: need en peasant and castling through check

        return result;
    }

    private static string InvokeNeuralNetwork(List<FutureCalculation> preCalculations)
    {
        var nnInputFilePath = "C:/Users/bruna/source/repos/AiChessEngine/neuralnetwork/input.json";
        var nnOutputFilePath = "C:/Users/bruna/source/repos/AiChessEngine/neuralnetwork/output.json";

        if (File.Exists(nnInputFilePath))
            File.Delete(nnInputFilePath);

        if (File.Exists(nnOutputFilePath))
            File.Delete(nnOutputFilePath);

        using (var sw = new StreamWriter(nnInputFilePath, true))
        {
            var serializedPayload = JsonSerializer.Serialize(preCalculations);
            sw.WriteLine(serializedPayload);
        }

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python";
        start.Arguments = "forward.py";
        start.UseShellExecute = true; // Do not use OS shell.
        start.WorkingDirectory = "C:/Users/bruna/source/repos/AiChessEngine/neuralnetwork";
        start.CreateNoWindow = false; // Don't create new window.
        using (Process process = Process.Start(start))
        {
            // Wait for the python script to finish
            process.WaitForExit();
        }

        if (!File.Exists(nnOutputFilePath))
            throw new ApplicationException("python script didn't generate any output file");

        return File.ReadAllText(nnOutputFilePath);
    }

    private static void AnalyzeCalculations(List<FutureCalculation> calculations)
    {
        Console.WriteLine($"Model saw {calculations.Count} potential moves.");
        foreach(var calculation in calculations)
        {
            Console.WriteLine($"\tMin and Max for {calculation.Move.MoveName}: {calculation.MinAndMaxEvals().Item1}, {calculation.MinAndMaxEvals().Item2}");
        }
    }

    private static string SerializePosition(Move move, bool readable = false)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if(readable)
                {
                    if (move.NewPosition.ToIntMatrix()[i, j] < 0)
                    {
                        sb.Append(move.NewPosition.ToIntMatrix()[i, j] + ",");
                    }
                    else
                    {
                        sb.Append(" " + move.NewPosition.ToIntMatrix()[i, j] + ",");
                    }
                }
                else
                {
                    sb.Append(move.NewPosition.ToIntMatrix()[i, j] + ",");
                }
            }

            if(readable)
                sb.Append("\n");
        }

        return sb.ToString().TrimEnd(',');
    }
}