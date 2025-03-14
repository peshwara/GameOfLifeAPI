namespace GameOfLifeAPI.Services
{
    public class GameOfLife
    {
        public static int[,] GetNextGeneration(int[,] currentState)
        {
            int rows = currentState.GetLength(0);
            int cols = currentState.GetLength(1);
            var nextState = new int[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int liveNeighbors = CountLiveNeighbors(currentState, row, col, rows, cols);

                    if (currentState[row, col] == 1)
                    {
                        // Cell is alive
                        nextState[row, col] = (liveNeighbors == 2 || liveNeighbors == 3) ? 1 : 0;
                    }
                    else
                    {
                        // Cell is dead
                        nextState[row, col] = (liveNeighbors == 3) ? 1 : 0;
                    }
                }
            }

            return nextState;
        }

        private static int CountLiveNeighbors(int[,] board, int row, int col, int rows, int cols)
        {
            int count = 0;
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r >= 0 && r < rows && c >= 0 && c < cols && !(r == row && c == col))
                    {
                        if (board[r, c] == 1)
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }
    }

}
