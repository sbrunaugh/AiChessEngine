using DecisionEngine.Helpers;
using DecisionEngine;

namespace DecisionEngineTests
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void StartingPositionTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = QueenHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(0, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void a4a5h4h5PositionTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 2, 3, 0, 9, 3, 2, 5 },
                {  1, 1, 1, 0, 1, 1, 1, 1 },
                {  0, 0, 0, 8, 0, 0, 0, 0 },
                {  0, 0, 0, 1, 0, 0, 0, 0 },
                {  0, 0, 0,-1, 0, 0, 0, 0 },
                {  0, 0, 0,-8, 0, 0, 0, 0 },
                { -1,-1,-1, 0,-1,-1,-1,-1 },
                { -5,-2,-3, 0,-9,-3,-2,-5 },
            };

            // Act
            var potentialNewPositions = QueenHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(16, potentialNewPositions.Count);
        }
    }
}
