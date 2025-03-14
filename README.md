# Game of Life API

This is a simple REST API for the **Game of Life** simulation, which allows users to
- Create a board
- Get the current state
- Get next generations(state)
- Get the final state after a specified number of generations.


## Instructions to Run the Application
dotnet run

## Restore dependancy
dotnet restore

## Clean and Build
dotnet clean
dotnet build


API Endpoints

1. Create a New Board
Endpoint: POST /api/gameoflife/boards/create
Request Body:
json
Copy
{
  "initialState": [
    [1, 0, 0],
    [0, 1, 1],
    [1, 1, 0]
  ]
}
Response:
200 OK: Board created successfully with ID.
400 Bad Request: If the initial state is invalid.

2. Get the Current State of the Board
Endpoint: GET /api/gameoflife/boards/{boardId}
Parameters:
boardId (int): The ID of the board.
Response:
200 OK: Returns the current state of the board.
404 Not Found: If the board with the given ID does not exist.

3. Get the next state of the board (next generation)
Endpoint: GET /api/gameoflife/boards/{boardId}/next
Parameters:
boardId (int): The ID of the board.
Response:
200 OK: Returns the next state of the board.
400 Bad Request: If the board does not exist.

4. Get the state of the board after a specific number of generations
Endpoint: GET /api/gameoflife/boards/{boardId}/generations/{generations}
Parameters:
boardId (int): The ID of the board.
generations (int): The number of generations to evolve the board.
Response:
200 OK: Returns the board's state after the specified number of generations.
400 Bad Request: If the board does not exist.

5. Get the final state of the board after a maximum number of generations
Endpoint: GET /api/gameoflife/boards/{boardId}/final
Parameters:
boardId (int): The ID of the board.
maxGenerations (int): The maximum number of generations to evolve the board.
Response:
200 OK: Returns the final state of the board if it stabilizes within the specified number of generations.
422 Unprocessable Entity: If the board does not stabilize within the maximum allowed generations.


Testing Instructions:
1. Running Unit Tests with dotnet test
Run the following command in the terminal:
dotnet test

2. Testing with GameOfLifeAPI.http

You can also test the API using a .http file with Visual Studio Code (or another editor that supports HTTP requests).
Below are some examples:

To use this GameOfLifeAPI.http file:

- Install the REST Client extension in Visual Studio Code (or use a similar HTTP client extension in your IDE).
- Open the .http file.
- Use the "Send Request" feature to send each of the requests.
- The results will be displayed directly in the editor.



# TODO
Scope for improvements
- Use Byte[] instead of int[,] or string for more efficient storage
- Add Logs with specializded tools like Elastic Search/ DataDog trace for monitoring and observability 
- Could be containerized for plateform independace
- Custom Exceptions
- Use of non-relational DB like MongoDB, Redis or S3/Azure to store grid Datastructure for Flexiable Storage, Fast Caching or scalability as necessary. But Querability will be compromised 