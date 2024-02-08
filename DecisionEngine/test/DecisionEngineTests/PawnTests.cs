using DecisionEngine;
using DecisionEngine.Helpers;

namespace DecisionEngineTests
{
    [TestClass]
    public class PawnTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void BeginningPawnMovesTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = PawnHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(16, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void NoPawnMovesTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
            };

            // Act
            var potentialNewPositions = PawnHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(0, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void PawnsBlockedTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  1, 1, 1, 1, 1, 1, 1, 1 },
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
                { -1,-1,-1,-1,-1,-1,-1,-1 },
            };

            // Act
            var potentialNewPositions = PawnHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(0, potentialNewPositions.Count);
        }
    }
}