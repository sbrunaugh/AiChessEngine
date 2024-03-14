using DecisionEngine.Helpers;
using DecisionEngine.Models;
using RestSharp;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace DecisionEngine;

public class Program
{
    private const string workingDir = "C:/Users/bruna/source/repos/AiChessEngine/";
    static void Main(string[] args)
    {
        var inputArgs = ParseArgs(args);

        if (inputArgs.EvaluationType == EvaluationType.SingleLayer)
        {
            Task.Run(async () => {
                await EvaluateSingleLayerDeep(inputArgs.Position, inputArgs.Player);
            }).GetAwaiter().GetResult();
        }
        else
        {
            EvaluateTwoLayersDeep(inputArgs.Position, inputArgs.Player);
        }
    }

    private static InputArgs ParseArgs(string[] args)
    {
        if (args == null || args.Length != 3)
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

        Player modelPlayer;
        if (int.TryParse(args[1], out var playerInt))
        {
            modelPlayer = playerInt == 1 ? Player.White : Player.Black;
        }
        else
        {
            throw new ArgumentException(nameof(args));
        }

        if (args[2] != "1" && args[2] != "2")
            throw new ArgumentException(nameof(args));

        return new InputArgs
        {
            EvaluationType = args[2] == "1" ? EvaluationType.SingleLayer : EvaluationType.TwoLayers,
            Player = modelPlayer,
            Position = currentPosition
        };
    }

    public async static Task EvaluateSingleLayerDeep(int[,] currentPosition, Player modelPlayer)
    {
        var legalMoves = FindAllLegalMoves(currentPosition, modelPlayer, true);

        var preEvaluatedMoves = new List<EvaluatedMove>();
        foreach (var move in legalMoves)
        {
            preEvaluatedMoves.Add(new EvaluatedMove()
            {
                Evaluation = 0f,
                Move = move
            });
        };

        var evaluatedMoves = await InvokeNeuralNetwork(preEvaluatedMoves);

        //if(modelPlayer == Player.White)
        //{
        //    evaluatedMoves = evaluatedMoves.OrderByDescending(m => m.Evaluation).ToList();
        //}
        //else
        //{
        //    evaluatedMoves = evaluatedMoves.OrderBy(m => m.Evaluation).ToList();
        //}

        evaluatedMoves = evaluatedMoves.OrderByDescending(m => m.Evaluation).ToList();

        AnalyzeSingleLayerCalculations(preEvaluatedMoves);

        var bestMove = evaluatedMoves.First().Move;

        Log(bestMove.MoveName);
        Log(SerializePosition(bestMove, true));
    }

    private static void AnalyzeSingleLayerCalculations(List<EvaluatedMove> futureCalculations)
    {
        //var playerStr = futureCalculations[0].Move.Player == Player.White ? "white" : "black";
        //var enemyStr = playerStr == "white" ? "black" : "white";

        //Log($"Model (playing as {playerStr}) saw {futureCalculations.Count} potential moves.");
        //Log($"These are the Min and Max evaluations for {enemyStr}'s follow up moves:");

        //foreach (var calculation in futureCalculations)
        //{
        //    var thisMoveName = calculation.Move.MoveName;
        //    var minMax = calculation.MinAndMaxEvals();
        //    var minMoveName = calculation.FutureEvaluatedMoves.First(m => m.Evaluation == minMax.Item1).Move.MoveName;
        //    var maxMoveName = calculation.FutureEvaluatedMoves.First(m => m.Evaluation == minMax.Item2).Move.MoveName;
        //    Log($"\t{thisMoveName} | {minMax.Item1} ({minMoveName}) - {minMax.Item2} ({maxMoveName}).");
        //}
    }

    public static void EvaluateTwoLayersDeep(int[,] currentPosition, Player modelPlayer)
    {
        var legalMoves = FindAllLegalMoves(currentPosition, modelPlayer, true);

        var futureCalculations = new List<FutureCalculation>();
        var enemy = modelPlayer == Player.White ? Player.Black : Player.White;
        foreach (var move in legalMoves)
        {
            var x = new FutureCalculation()
            {
                Move = move,
                FutureEvaluatedMoves = new List<EvaluatedMove>()
            };

            var secondLayer = FindAllLegalMoves(x.Move.NewPosition.ToIntMatrix(), enemy, true);
            foreach (var nextMove in secondLayer)
            {
                var y = new EvaluatedMove()
                {
                    Move = nextMove,
                    Evaluation = 0f
                };
                x.FutureEvaluatedMoves.Add(y);
            }

            futureCalculations.Add(x);
        }

        futureCalculations = InvokeNeuralNetwork(futureCalculations);

        AnalyzeTwoLayerCalculations(futureCalculations);

        // best option for white is the move that results in a set of moves for black where the lowest of
        // that set is scored as high as possible. Highest of the lowest possibilities.
        Move bestMove;

        if (futureCalculations[0].Move.Player == Player.Black)
        {
            bestMove = futureCalculations.OrderByDescending(c => c.MinAndMaxEvals().Item2).First().Move;
        }
        else
        {
            bestMove = futureCalculations.OrderBy(c => c.MinAndMaxEvals().Item1).First().Move;
        }

        Log(bestMove.MoveName);
        Log(SerializePosition(bestMove, true));
    }

    private static void AnalyzeTwoLayerCalculations(List<FutureCalculation> futureCalculations)
    {
        var playerStr = futureCalculations[0].Move.Player == Player.White ? "white" : "black";
        var enemyStr = playerStr == "white" ? "black" : "white";

        Log($"Model (playing as {playerStr}) saw {futureCalculations.Count} potential moves.");
        Log($"These are the Min and Max evaluations for {enemyStr}'s follow up moves:");

        foreach (var calculation in futureCalculations)
        {
            var thisMoveName = calculation.Move.MoveName;
            var minMax = calculation.MinAndMaxEvals();
            var minMoveName = calculation.FutureEvaluatedMoves.First(m => m.Evaluation == minMax.Item1).Move.MoveName;
            var maxMoveName = calculation.FutureEvaluatedMoves.First(m => m.Evaluation == minMax.Item2).Move.MoveName;
            Log($"\t{thisMoveName} | {minMax.Item1} ({minMoveName}) - {minMax.Item2} ({maxMoveName}).");
        }
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

    private async static Task<List<EvaluatedMove>> InvokeNeuralNetwork(List<EvaluatedMove> evaluatedMoves)
    {
        var httpClient = new RestClient("http://localhost:5000/api");
        var postRequest = new RestRequest("forward", Method.Post);

        postRequest.AddBody(evaluatedMoves);

        var result = await httpClient.PostAsync(postRequest);

        var final = JsonSerializer.Deserialize<List<EvaluatedMove>>(result.Content);
        return final;
        //var nnInputFilePath = workingDir + "neuralnetwork/input.json";
        //var nnOutputFilePath = workingDir + "neuralnetwork/output.json";

        //if (File.Exists(nnInputFilePath))
        //    File.Delete(nnInputFilePath);

        //if (File.Exists(nnOutputFilePath))
        //    File.Delete(nnOutputFilePath);

        //using (var sw = new StreamWriter(nnInputFilePath, true))
        //{
        //    var serializedPayload = JsonSerializer.Serialize(evaluatedMoves);
        //    sw.WriteLine(serializedPayload);
        //}

        //ProcessStartInfo start = new ProcessStartInfo();
        //start.FileName = "python";
        //start.Arguments = $"forward_single.py";
        //start.UseShellExecute = true; // Do not use OS shell.
        //start.WorkingDirectory = workingDir + "neuralnetwork";
        //start.CreateNoWindow = false; // Don't create new window.
        //using (Process process = Process.Start(start))
        //{
        //    // Wait for the python script to finish
        //    process.WaitForExit();
        //}

        //if (!File.Exists(nnOutputFilePath))
        //    throw new ApplicationException("python script didn't generate any output file");

        //var outputText = File.ReadAllText(nnOutputFilePath);
        //return JsonSerializer.Deserialize<List<EvaluatedMove>>(outputText);
    }

    private static List<FutureCalculation> InvokeNeuralNetwork(List<FutureCalculation> futureCalculations)
    {
        var nnInputFilePath = workingDir + "neuralnetwork/input.json";
        var nnOutputFilePath = workingDir + "neuralnetwork/output.json";

        if (File.Exists(nnInputFilePath))
            File.Delete(nnInputFilePath);

        if (File.Exists(nnOutputFilePath))
            File.Delete(nnOutputFilePath);

        using (var sw = new StreamWriter(nnInputFilePath, true))
        {
            var serializedPayload = JsonSerializer.Serialize(futureCalculations);
            sw.WriteLine(serializedPayload);
        }

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python";
        start.Arguments = $"forward.py";
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
        return JsonSerializer.Deserialize<List<FutureCalculation>>(outputText);
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

    private static void Log(string message)
    {
        Console.WriteLine(message);
    }
}