using System;
using Xunit;
using GameOfLifeAPI.Services;

namespace Game.Tests.Services;

public class GameOfLifeServiceTests
{
    [Fact]
    public void GetNextGeneration()
    {
        var currentState = new int[,]
        {
            { 0, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        };

        var actualNextState = GameOfLife.GetNextGeneration(currentState);
        var expectedNextState =new int[,]
        {
            { 0, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        };
        Assert.Equal(expectedNextState, actualNextState);

    }

    
    [Fact]
        public void TestSingleCellAliveDiesWithNoNeighbors()
        {
            // Arrange: A single cell that's alive with no neighbors.
            int[,] currentState = new int[3, 3] 
            {
                { 0, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 0 }
            };

            // Act: Get the next generation.
            int[,] nextState = GameOfLife.GetNextGeneration(currentState);

            // Assert: The cell should be dead in the next generation.
            Assert.Equal(0, nextState[1, 1]);
        }

        [Fact]
        public void TestDeadCellWithThreeNeighborsComesToLife()
        {
            // Arrange: A dead cell surrounded by exactly three live neighbors.
            int[,] currentState = new int[3, 3]
            {
                { 0, 0, 0 },
                { 1, 0, 1 },
                { 0, 1, 0 }
            };

            // Act: Get the next generation.
            int[,] nextState = GameOfLife.GetNextGeneration(currentState);



            // Assert: The dead cell should come to life.
            Assert.Equal(1, nextState[1, 1]);
        }

        [Fact]
        public void TestAliveCellWithTwoNeighborsStaysAlive()
        {
            // Arrange: An alive cell with exactly two live neighbors.
            int[,] currentState = new int[3, 3]
            {
                { 0, 1, 0 },
                { 1, 1, 1 },
                { 0, 0, 0 }
            };

            // Act: Get the next generation.
            int[,] nextState = GameOfLife.GetNextGeneration(currentState);

            // Assert: The cell should stay alive.
            Assert.Equal(1, nextState[1, 1]);
        }

        [Fact]
        public void TestAliveCellWithMoreThanThreeNeighborsDies()
        {
            // Arrange: An alive cell with more than three live neighbors (overpopulation).
            int[,] currentState = new int[3, 3]
            {
                { 1, 1, 1 },
                { 1, 1, 1 },
                { 1, 0, 1 }
            };

            // Act: Get the next generation.
            int[,] nextState = GameOfLife.GetNextGeneration(currentState);

            // Assert: The cell should die due to overpopulation.
            Assert.Equal(0, nextState[1, 1]);
        }

        [Fact]
        public void TestLargeGrid()
        {
            // Arrange: A more complex board with multiple live and dead cells.
            int[,] currentState = new int[5, 5]
            {
                { 0, 1, 0, 1, 0 },
                { 1, 1, 1, 1, 1 },
                { 0, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 0 },
                { 1, 0, 1, 1, 0 }
            };

            var expectedNextState =new int[,]
            {
                { 1, 1, 0, 1, 1 },
                { 1, 0, 0, 1, 1 },
                { 1, 0, 0, 0, 1 },
                { 0, 0, 0, 1, 0 },
                { 0, 1, 1, 1, 0 }
            };

            // Act: Get the next generation.
            int[,] nextState = GameOfLife.GetNextGeneration(currentState);

            // Assert: Verify the expected changes in the grid.
            Assert.Equal(expectedNextState, nextState);
        }
}