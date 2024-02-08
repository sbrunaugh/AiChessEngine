using DecisionEngine;
using DecisionEngine.Helpers;

namespace DecisionEngineTests
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void StartingPositionTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = BishopHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(0, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void e4e5d4d5PositionTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  1, 1, 1, 0, 0, 1, 1, 1 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 1, 1, 0, 0, 0 },
                {  0, 0, 0,-1,-1, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -1,-1,-1, 0, 0,-1,-1,-1 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
            };

            // Act
            var potentialNewPositions = BishopHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(10, potentialNewPositions.Count);
        }
    }
}
