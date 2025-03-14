# Game of Life API

This is a simple REST API for the **Game of Life** simulation, which allows users to:
- Create a board
- Get the current state
- Get next generations (state)
- Get the final state after a specified number of generations.

## Instructions to Run the Application
```sh
dotnet run
```

## Restore Dependency
```sh
dotnet restore
```

## Clean and Build
```sh
dotnet clean
dotnet build
```

## API Endpoints

### 1. Create a New Board
**Endpoint:** `POST /api/gameoflife/boards/create`

**Request Body:**
```json
{
  "initialState": [
    [1, 0, 0],
    [0, 1, 1],
    [1, 1, 0]
  ]
}
```

**Response:**
- `200 OK`: Board created successfully with ID.
- `400 Bad Request`: If the initial state is invalid.

---

### 2. Get the Current State of the Board
**Endpoint:** `GET /api/gameoflife/boards/{boardId}`

**Parameters:**
- `boardId` (int): The ID of the board.

**Response:**
- `200 OK`: Returns the current state of the board.
- `404 Not Found`: If the board with the given ID does not exist.

---

### 3. Get the Next State of the Board (Next Generation)
**Endpoint:** `GET /api/gameoflife/boards/{boardId}/next`

**Parameters:**
- `boardId` (int): The ID of the board.

**Response:**
- `200 OK`: Returns the next state of the board.
- `400 Bad Request`: If the board does not exist.

---

### 4. Get the State of the Board After a Specific Number of Generations
**Endpoint:** `GET /api/gameoflife/boards/{boardId}/generations/{generations}`

**Parameters:**
- `boardId` (int): The ID of the board.
- `generations` (int): The number of generations to evolve the board.

**Response:**
- `200 OK`: Returns the board's state after the specified number of generations.
- `400 Bad Request`: If the board does not exist.

---

### 5. Get the Final State of the Board After a Maximum Number of Generations
**Endpoint:** `GET /api/gameoflife/boards/{boardId}/final`

**Parameters:**
- `boardId` (int): The ID of the board.
- `maxGenerations` (int): The maximum number of generations to evolve the board.

**Response:**
- `200 OK`: Returns the final state of the board if it stabilizes within the specified number of generations.
- `422 Unprocessable Entity`: If the board does not stabilize within the maximum allowed generations.

---

## Testing Instructions

### 1. Running Unit Tests with `dotnet test`
Run the following command in the terminal:
```sh
dotnet test
```

### 2. Testing with `GameOfLifeAPI.http`
You can also test the API using an `.http` file with Visual Studio Code (or another editor that supports HTTP requests).

**To use this `GameOfLifeAPI.http` file:**
- Install the **REST Client** extension in Visual Studio Code (or use a similar HTTP client extension in your IDE).
- Open the `.http` file.
- Use the "Send Request" feature to send each of the requests.
- The results will be displayed directly in the editor.

---

## TODO: Scope for Improvements

- Use `byte[]` instead of `int[,]` or `string` for more efficient storage.
- Add logging with specialized tools like **Elastic Search** or **DataDog Trace** for monitoring and observability.
- Containerize the application for platform independence.
- Implement custom exceptions.
- Utilize non-relational databases like **MongoDB, Redis, or S3/Azure** to store the grid data structure for flexible storage, fast caching, or scalability as necessary (though queryability may be compromised).
