namespace GameOfLifeAPI.Conteollers
{
    using System.Text.Json.Nodes;
    using GameOfLifeAPI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Any;

    [ApiController]
    [Route("api/[controller]")]
    public class GameOfLifeController : ControllerBase
    {
         private readonly IGameOfLifeService _gameOfLifeService;
        private readonly ILogger<GameOfLifeController> _logger;

        public GameOfLifeController(IGameOfLifeService gameOfLifeService, ILogger<GameOfLifeController> logger)
        {
            _gameOfLifeService = gameOfLifeService;
            _logger = logger;
        }


        // POST api/gameoflife/create
        [HttpPost("boards/create")]
        public async Task<IActionResult> CreateBoardAsync([FromBody] JsonNode initialStateNode)
        {

            // Extract the initialState property
            var initialStateArray = initialStateNode["initialState"] as JsonArray;
            if (initialStateArray == null || initialStateArray.Count == 0 || initialStateArray[0].AsArray().Count == 0)
            {
                return BadRequest("Invalid initial state.");
            }

            int[,] multiDimensionalArray;
            try
            {
                 multiDimensionalArray = _gameOfLifeService.ValidateAndConvert(initialStateArray);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            try
            {
                var boardId = await _gameOfLifeService.CreateBoardAsync(multiDimensionalArray);
                return Ok("Board created with Id: " + boardId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("boards/{boardId}")]
        public async Task<IActionResult> GetCurrentBoardState(int boardId)
        {   
            try
            {
                var currentState = await _gameOfLifeService.GetBoardAsync(boardId);
                return Ok(currentState);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("boards/{boardId}/next")]
        public async Task<IActionResult> GetNextBoardState(int boardId)
        {
            try
            {
                var nextState = await _gameOfLifeService.GetNextGenerationAsync(boardId);
                return Ok(nextState);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("boards/{boardId}/generations/{generations}")]
        public async Task<IActionResult> GetStatesAtGeneration(int boardId, int generations)
        {
            try
            {
                var futureState = await _gameOfLifeService.GetStatesAtGenerationAsync(boardId, generations);
                return Ok(futureState);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("boards/{boardId}/final")]
        public async Task<IActionResult> GetFinalState(int boardId, int maxGenerations)
        {
            try
            {
                var state = await _gameOfLifeService.GetFinalStateAsync(boardId, maxGenerations);
                return Ok(state);
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }

}