using DecisionEngine.Helpers;
using DecisionEngine;

namespace DecisionEngineTests
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void StartingPositionTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = KnightHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(4, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void FourKnightsPositionTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 0, 3, 8, 9, 3, 0, 5 },
                {  1, 1, 1, 1, 1, 1, 1, 1 },
                {  0, 0, 2, 0, 0, 2, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0,-2, 0, 0,-2, 0, 0 },
                { -1,-1,-1,-1,-1,-1,-1,-1 },
                { -5, 0,-3,-8,-9,-3, 0,-5 },
            };

            // Act
            var potentialNewPositions = KnightHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(10, potentialNewPositions.Count);
        }
    }
}
