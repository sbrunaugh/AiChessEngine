using DecisionEngine.Helpers;
using DecisionEngine.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DecisionEngine;

public class Program
{
    private const string workingDir = "C:/Users/sbrunaugh/source/repos/Personal/AiChessEngine/";
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

        //var preCalculations = new List<FutureCalculation>();
        //var enemy = player == Player.White ? Player.Black : Player.White;
        //foreach(var move in legalMoves)
        //{
        //    var x = new FutureCalculation()
        //    {
        //        Move = move,
        //        FutureEvaluatedMoves = new List<EvaluatedMove>()
        //    };

        //    var secondLayer = FindAllLegalMoves(x.Move.NewPosition.ToIntMatrix(), enemy, true);
        //    foreach(var nextMove in secondLayer)
        //    {
        //        var y = new EvaluatedMove()
        //        {
        //            Move = nextMove,
        //            Evaluation = 0f
        //        };
        //        x.FutureEvaluatedMoves.Add(y);
        //    }

        //    preCalculations.Add(x);
        //}

        var preEvaluatedMoves = new List<EvaluatedMove>();
        foreach (var move in legalMoves)
        {
            preEvaluatedMoves.Add(new EvaluatedMove()
            {
                Evaluation = 0f,
                Move = move
            });
        }

        var evaluatedMoves = InvokeNeuralNetwork(preEvaluatedMoves, player);
        //var computedCalculations = JsonSerializer.Deserialize<List<FutureCalculation>>(outputText);

        AnalyzeCalculations(evaluatedMoves);

        // best option for white is the move that results in a set of moves for black where the lowest of
        // that set is scored as high as possible. Highest of the lowest possibilities.
        var bestMove = player == Player.White
            ? evaluatedMoves.OrderByDescending(cc => cc.Evaluation).First().Move
            : evaluatedMoves.OrderBy(cc => cc.Evaluation).First().Move;

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

    private static List<EvaluatedMove> InvokeNeuralNetwork(List<EvaluatedMove> moves, Player player)
    {
        var nnInputFilePath = workingDir + "neuralnetwork/input.json";
        var nnOutputFilePath = workingDir + "neuralnetwork/output.json";

        if (File.Exists(nnInputFilePath))
            File.Delete(nnInputFilePath);

        if (File.Exists(nnOutputFilePath))
            File.Delete(nnOutputFilePath);

        using (var sw = new StreamWriter(nnInputFilePath, true))
        {
            var serializedPayload = JsonSerializer.Serialize(moves);
            sw.WriteLine(serializedPayload);
        }

        var playerStr = player == Player.White ? "white" : "black";

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python";
        start.Arguments = $"forward.py {playerStr}";
        start.UseShellExecute = true; // Do not use OS shell.
        start.WorkingDirectory = workingDir + "neuralnetwork";
        start.CreateNoWindow = false; // Don't create new window.
        using (Process process = Process.Start(start))
        {
            // Wait for the python script to finish
            process.WaitForExit();
        }

        if (!File.Exists(nnOutputFilePath))
            throw new ApplicationException("python script didn't generate any output file");

        var outputText = File.ReadAllText(nnOutputFilePath);
        return JsonSerializer.Deserialize<List<EvaluatedMove>>(outputText);
    }

    private static void AnalyzeCalculations(List<EvaluatedMove> evaluatedMoves)
    {
        evaluatedMoves = evaluatedMoves[0].Move.Player == Player.White
            ? evaluatedMoves.OrderByDescending(m => m.Evaluation).ToList()
            : evaluatedMoves.OrderBy(m => m.Evaluation).ToList();

        Console.WriteLine($"Model saw {evaluatedMoves.Count} potential moves.");
        foreach(var eval in evaluatedMoves)
        {
            Console.WriteLine($"\t{eval.Move.MoveName}: {eval.Evaluation}");
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