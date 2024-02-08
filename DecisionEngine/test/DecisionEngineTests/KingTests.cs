using DecisionEngine;
using DecisionEngine.Helpers;

namespace DecisionEngineTests
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void StartingPositionTest(Player player)
        {
            // Arrange
            var board = BoardHelper.GenerateFreshBoard();

            // Act
            var potentialNewPositions = KingHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(0, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void e4e5PositionTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  5, 2, 3, 8, 9, 3, 2, 5 },
                {  1, 1, 1, 1, 0, 1, 1, 1 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 1, 0, 0, 0 },
                {  0, 0, 0, 0,-1, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                { -1,-1,-1,-1, 0,-1,-1,-1 },
                { -5,-2,-3,-8,-9,-3,-2,-5 },
            };

            // Act
            var potentialNewPositions = KingHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(1, potentialNewPositions.Count);
        }

        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void KingCanCaptureTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  9, 0, 0, 0, 0, 0, 0, 0 },
                { -2, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 2 },
                {  0, 0, 0, 0, 0, 0, 0,-9 },
            };

            // Act
            var potentialNewPositions = KingHelper.FindAllLegalMoves(board, player);

            // Assert
            Assert.AreEqual(3, potentialNewPositions.Count);
        }



        [TestMethod]
        [DataRow(Player.White)]
        [DataRow(Player.Black)]
        public void KingCantMoveIntoCheckTest(Player player)
        {
            // Arrange
            var board = new int[8, 8]
            {
                {  9, 0, 0, 0, 0, 0, 5, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0, 0, 0, 0, 0, 0, 0, 0 },
                {  0,-5, 0, 0, 0, 0, 0,-9 },
            };

            // Act
            var potentialNewPositions = KingHelper.FindAllLegalMoves(board, player);
            MoveHelper.FilterOutMovesResultingInCheck(potentialNewPositions, player);

            // Assert
            Assert.AreEqual(1, potentialNewPositions.Count);
        }
    }
}
