using DecisionEngine.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text;

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

        var nnInputFilePath = "C:/Users/bruna/source/repos/AiChessEngine/neuralnetwork/input.csv";
        var nnOutputFilePath = "C:/Users/bruna/source/repos/AiChessEngine/neuralnetwork/output.txt";

        if (File.Exists(nnInputFilePath))
            File.Delete(nnInputFilePath);

        if (File.Exists(nnOutputFilePath))
            File.Delete(nnOutputFilePath);

        using (var sw = new StreamWriter(nnInputFilePath, true))
        {
            foreach (var move in legalMoves)
            {
                try
                {
                    var serializedPosition = SerializePosition(move);
                    sw.WriteLine(serializedPosition);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
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

        var outputText = File.ReadAllText(nnOutputFilePath).TrimEnd().Split("\r\n");

        var indexedEvaluations = new List<float>();
        foreach(var line in outputText)
        {
            indexedEvaluations.Add(float.Parse(line));
        }

        Assert.AreEqual(legalMoves.Count, indexedEvaluations.Count);

        var bestMoveIndex = MoveHelper.PickRandomBestEvaluationIndex(indexedEvaluations, player);
        var moveName = MoveHelper.GenerateMoveName(currentPosition, legalMoves[bestMoveIndex], player);

        Console.WriteLine(moveName + ":");
        Console.WriteLine(SerializePosition(legalMoves[bestMoveIndex], true));
    }

    public static List<int[,]> FindAllLegalMoves(int[,] board, Player player, bool includeCheckCheck = false)
    {
        var result = new List<int[,]>();

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

    private static string SerializePosition(int[,] position, bool readable = false)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if(readable)
                {
                    if (position[i, j] < 0)
                    {
                        sb.Append(position[i, j] + ",");
                    }
                    else
                    {
                        sb.Append(" " + position[i, j] + ",");
                    }
                }
                else
                {
                    sb.Append(position[i, j] + ",");
                }
            }

            if(readable)
                sb.Append("\n");
        }

        return sb.ToString().TrimEnd(',');
    }
}