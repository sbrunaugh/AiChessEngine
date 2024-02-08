using DecisionEngine.Helpers;
using DecisionEngine;

namespace DecisionEngineTests
{
    [TestClass]
    public class CombinationTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void StartingPositionTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = Program.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(20, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void PawnsForwardPositionTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  1, 1, 1, 1, 1, 1, 1, 1 },
                { -1,-1,-1,-1,-1,-1,-1,-1 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
            };

            // Act
            var potentialNewPositions = Program.FindAllLegalMoves(board, player);

            // Assert
            var pawnMoves = 14;
            var knightMoves = 6;
            var bishopMoves = 8;
            var rookMoves = 4;
            var queenMoves = 6;
            var kingMoves = 3;
            var totalMoves = pawnMoves + knightMoves + bishopMoves + rookMoves + queenMoves + kingMoves;

            Assert.AreEqual(totalMoves, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void CastleTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 0, 0, 0, 9, 0, 0, 5 },
                {  1, 1, 1, 1, 1, 1, 1, 1 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -1,-1,-1,-1,-1,-1,-1,-1 },
                { -5, 0, 0, 0,-9, 0, 0,-5 },
            };

            // Act
            var potentialNewPositions = MoveHelper.FindAllLegalCastles(board, player);

            // Assert
            Assert.AreEqual(2, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void CheckTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  9, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -9, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
            };

            // Act
            var potentialNewPositions = Program.FindAllLegalMoves(board, player, true);

            // Assert
            Assert.AreEqual(3, potentialNewPositions.Count);
        }
    }
}
