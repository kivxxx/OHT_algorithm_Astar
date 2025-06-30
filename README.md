# OHT Route Planning Algorithm

This project implements a route planning algorithm for an Overhead Hoist Transport (OHT) system using the A* search algorithm. It calculates the shortest path between two points in a given layout, which is defined in a JSON file.

## Features

-   **A* Algorithm**: Utilizes the A* search algorithm with the Euclidean distance heuristic to find the optimal path.
-   **JSON-based Layout**: The OHT system's layout, including nodes (stations, switches, via points) and edges, is defined in a flexible JSON format.
-   **Interactive Console**: An interactive console allows users to load a layout, view available nodes, and calculate routes between any two nodes.
-   **Flexible Output**: The calculated route can be displayed in both a detailed and a simplified JSON format.

## How to Use

### Running the Application

1.  **Download the Release**: Go to the [Releases page](https://github.com/kivxxx/OHT_algorithm_Astar/releases) and download the `OHTAlgorithm.dll` from the latest release (e.g., `V1.0.0`).
2.  **Open a Terminal**: Navigate to the directory where you downloaded the `OHTAlgorithm.dll`.
3.  **Run the Application**: Execute the application using the .NET runtime:
    ```bash
    dotnet OHTAlgorithm.dll
    ```

### Interacting with the Application

Once the application starts, it will automatically load the predefined layout and display available nodes.

1.  **Enter Start Node ID**: The console will prompt you to enter the ID of the starting node (e.g., `H1`, `D1`, `V1`). Type the ID and press Enter.
2.  **Enter End Node ID**: Next, enter the ID of the ending node. Type the ID and press Enter.
3.  **View Route**: The application will calculate and display the shortest route, showing each step with its node ID and side.
4.  **Choose Output Format**: After displaying the route, you will be prompted to choose an output format:
    -   `1`: Full JSON (includes `type` and `switch-option`)
    -   `2`: Simplified JSON (includes `no`, `id`, `side`)
    -   `3`: Do not display JSON
    Enter your choice (1, 2, or 3) and press Enter.
5.  **Continue or Exit**: You can continue to calculate more routes by entering new start and end node IDs, or type `exit` when prompted for the start node ID to quit the application.

## Code Structure

-   `OHTRouteCalculator.cs`: Contains the core logic for the A* algorithm and route calculation.
-   `CompleteTest.cs`: Provides an interactive console for testing the route calculator and defines the sample layout.
-   `OHTAlgorithm.csproj`: The C# project file.