namespace GameOfLifeAPI.Services
{    
    public interface IGameOfLifeService
    {
        Task<int> CreateBoardAsync(int[,] initialState);
        Task<string> GetBoardAsync(int boardId);
        Task<string> GetNextGenerationAsync(int boardId);
        Task<string> GetStatesAtGenerationAsync(int boardId, int generations);
        Task<string> GetFinalStateAsync(int boardId, int maxGenerations);
    }

}
