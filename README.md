# OHT Route Planning Algorithm

This project implements a route planning algorithm for an Overhead Hoist Transport (OHT) system using the A* search algorithm. It calculates the shortest path between two points in a given layout, which is defined in a JSON file.

## Features

-   **A* Algorithm**: Utilizes the A* search algorithm with the Euclidean distance heuristic to find the optimal path.
-   **JSON-based Layout**: The OHT system's layout, including nodes (stations, switches, via points) and edges, is defined in a flexible JSON format.
-   **Interactive Console**: An interactive console allows users to load a layout, view available nodes, and calculate routes between any two nodes.
-   **Flexible Output**: The calculated route can be displayed in both a detailed and a simplified JSON format.

## How to Use

1.  **Build the project**: Compile the C# code using a .NET compiler.
2.  **Run the application**: Execute the compiled application.
3.  **Load Layout**: The application will automatically load the layout defined in `CompleteTest.cs`.
4.  **Calculate Route**:
    -   The console will display a list of all available nodes.
    -   Enter the ID of the starting node.
    -   Enter the ID of the ending node.
    -   The application will display the calculated route, including the steps, node types, and sides.
    -   You can then choose to view the route in a detailed or simplified JSON format.

## Code Structure

-   `OHTRouteCalculator.cs`: Contains the core logic for the A* algorithm and route calculation.
-   `CompleteTest.cs`: Provides an interactive console for testing the route calculator.
-   `OHTAlgorithm.csproj`: The C# project file.
