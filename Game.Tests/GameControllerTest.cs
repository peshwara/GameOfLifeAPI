using System.Text.Json.Nodes;
using GameOfLifeAPI.Conteollers;
using GameOfLifeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GameOfLifeAPI.Tests
{
    public class GameOfLifeControllerTests
    {
        private readonly Mock<IGameOfLifeService> _mockGameOfLifeService;
        private readonly GameOfLifeController _controller;

        public GameOfLifeControllerTests()
        {
            _mockGameOfLifeService = new Mock<IGameOfLifeService>();
            _controller = new GameOfLifeController(_mockGameOfLifeService.Object, Mock.Of<ILogger<GameOfLifeController>>());
        }

        [Fact]
        public async Task CreateBoardAsync_ReturnsOk_WhenValidState()
        {
            // Arrange
            var initialStateNode = JsonNode.Parse(@"{ ""initialState"": [[1, 0], [0, 1]] }");
            _mockGameOfLifeService.Setup(service => service.CreateBoardAsync(It.IsAny<int[,]>())).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateBoardAsync(initialStateNode);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Board created with Id: 1", okResult.Value);
        }

        [Fact]
        public async Task CreateBoardAsync_ReturnsBadRequest_WhenInvalidState()
        {
            // Arrange
            var initialStateNode = JsonNode.Parse(@"{ ""initialState"": [] }");

            // Act
            var result = await _controller.CreateBoardAsync(initialStateNode);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid initial state.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCurrentBoardState_ReturnsOk_WhenBoardExists()
        {
            // Arrange
            var boardId = 1;
            var state = "[[1, 0], [0, 1]]";
            _mockGameOfLifeService.Setup(service => service.GetBoardAsync(boardId)).ReturnsAsync(state);

            // Act
            var result = await _controller.GetCurrentBoardState(boardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(state, okResult.Value);
        }

        [Fact]
        public async Task GetCurrentBoardState_ReturnsNotFound_WhenBoardDoesNotExist()
        {
            // Arrange
            var boardId = 999;
            _mockGameOfLifeService.Setup(service => service.GetBoardAsync(boardId)).ThrowsAsync(new InvalidOperationException("Board not found."));

            // Act
            var result = await _controller.GetCurrentBoardState(boardId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Board not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetNextBoardState_ReturnsOk_WhenBoardExists()
        {
            // Arrange
            var boardId = 1;
            var nextState = "[[0, 1], [1, 0]]";
            _mockGameOfLifeService.Setup(service => service.GetNextGenerationAsync(boardId)).ReturnsAsync(nextState);

            // Act
            var result = await _controller.GetNextBoardState(boardId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(nextState, okResult.Value);
        }

        [Fact]
        public async Task GetNextBoardState_ReturnsBadRequest_WhenBoardDoesNotExist()
        {
            // Arrange
            var boardId = 999;
            _mockGameOfLifeService.Setup(service => service.GetNextGenerationAsync(boardId)).ThrowsAsync(new InvalidOperationException("Board not found."));

            // Act
            var result = await _controller.GetNextBoardState(boardId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Board not found.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetStatesAtGeneration_ReturnsOk_WhenBoardExists()
        {
            // Arrange
            var boardId = 1;
            var generations = 3;
            var futureState = "[[1, 1], [1, 1]]";
            _mockGameOfLifeService.Setup(service => service.GetStatesAtGenerationAsync(boardId, generations)).ReturnsAsync(futureState);

            // Act
            var result = await _controller.GetStatesAtGeneration(boardId, generations);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(futureState, okResult.Value);
        }

        [Fact]
        public async Task GetStatesAtGeneration_ReturnsBadRequest_WhenBoardDoesNotExist()
        {
            // Arrange
            var boardId = 999;
            var generations = 3;
            _mockGameOfLifeService.Setup(service => service.GetStatesAtGenerationAsync(boardId, generations)).ThrowsAsync(new InvalidOperationException("Board not found."));

            // Act
            var result = await _controller.GetStatesAtGeneration(boardId, generations);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Board not found.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetFinalState_ReturnsOk_WhenBoardExists()
        {
            // Arrange
            var boardId = 1;
            var maxGenerations = 5;
            var finalState = "[[0, 0], [0, 0]]";
            _mockGameOfLifeService.Setup(service => service.GetFinalStateAsync(boardId, maxGenerations)).ReturnsAsync(finalState);

            // Act
            var result = await _controller.GetFinalState(boardId, maxGenerations);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(finalState, okResult.Value);
        }

        [Fact]
        public async Task GetFinalState_ReturnsUnprocessableEntity_WhenBoardDoesNotExist()
        {
            // Arrange
            var boardId = 999;
            var maxGenerations = 5;
            _mockGameOfLifeService.Setup(service => service.GetFinalStateAsync(boardId, maxGenerations)).ThrowsAsync(new InvalidOperationException("Board not found."));

            // Act
            var result = await _controller.GetFinalState(boardId, maxGenerations);

            // Assert
            var unprocessableEntityResult = Assert.IsType<UnprocessableEntityObjectResult>(result);
            Assert.Equal("Board not found.", unprocessableEntityResult.Value);
        }
    }
}
