using System.Text.Json.Nodes;
using GameOfLifeAPI.Data;
using GameOfLifeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameOfLifeAPI.Services
{

    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly GameOfLifeContext _context;

        public GameOfLifeService(GameOfLifeContext context)
        {
            _context = context;
        }

        public async Task<int> CreateBoardAsync(int[,] initialState)
        {
            var board = new Board
            {
                Id = new Random().Next(1, int.MaxValue),  // Using an int for the Id
                Rows = initialState.GetLength(0),
                Columns = initialState.GetLength(1),
                StateArray = initialState  // This will handle serialization automatically
            };

            await _context.Boards.AddAsync(board);
            await _context.SaveChangesAsync();

            return board.Id;
        }

        public async Task<string> GetBoardAsync(int boardId)
        {
            var board = await _context.Boards.FindAsync(boardId);

            if (board == null)
            {
                throw new InvalidOperationException("Board not found.");
            }

            return board.State;
        }

        public async Task<string> GetNextGenerationAsync(int boardId)
        {   
            var board = await _context.Boards.FindAsync(boardId);

            if (board == null)
            {
                throw new InvalidOperationException("Board not found.");
            }
            board.StateArray = GameOfLife.GetNextGeneration(board.StateArray);  // Get next generation
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();

            return board.State;
        }

        public async Task<string> GetStatesAtGenerationAsync(int boardId, int generations)
        {
            var board = await _context.Boards.FindAsync(boardId);
            if (board == null) throw new InvalidOperationException("Board not found.");

            for (int i = 0; i < generations; i++)
            {
                board.StateArray = GameOfLife.GetNextGeneration(board.StateArray);
                // TODO: Could optimize at this stage to check if the state has stabilized or died
            }

            _context.Boards.Update(board);
            await _context.SaveChangesAsync();

            return board.State;
        }

        public async Task<string> GetFinalStateAsync(int boardId, int maxGenerations)
        {
            var board = await _context.Boards.FindAsync(boardId);
            if (board == null) throw new InvalidOperationException("Board not found.");

            int currentGeneration = 0;

            while (currentGeneration < maxGenerations)
            {
                int[,] nextState = GameOfLife.GetNextGeneration(board.StateArray);
                if (IsEqualState(board.StateArray, nextState))
                {
                    return board.State; // Final state found
                }

                board.StateArray = nextState;
                _context.Boards.Update(board);
                await _context.SaveChangesAsync();

                currentGeneration++;
            }

            throw new InvalidOperationException("Board did not stabilize within the maximum allowed generations.");
        }

        private bool IsEqualState(int[,] state1, int[,] state2)
        {
            for (int i = 0; i < state1.GetLength(0); i++)
            {
                for (int j = 0; j < state1.GetLength(1); j++)
                {
                    if (state1[i, j] != state2[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        int[,] IGameOfLifeService.ValidateAndConvert(JsonArray initialStateArray)
        {

            // Convert JsonArray to multi-dimensional array
            int rows = initialStateArray.Count;
            int cols = initialStateArray[0].AsArray().Count;
            int[,] multiDimensionalArray = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                var rowArray = initialStateArray[i].AsArray();
                for (int j = 0; j < cols; j++)
                {
                    multiDimensionalArray[i, j] = rowArray[j].GetValue<int>();
                }
            }

            return multiDimensionalArray;
            
        }
    }


}