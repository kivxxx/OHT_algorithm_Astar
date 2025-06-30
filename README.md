# OHT Route Planning Algorithm

This project implements a route planning algorithm for an Overhead Hoist Transport (OHT) system using the A* search algorithm. It calculates the shortest path between two points in a given layout, which is defined in a JSON file.

## Features

-	**A* Algorithm**: Utilizes the A* search algorithm with the Euclidean distance heuristic to find the optimal path.
-	**JSON-based Layout**: The OHT system's layout, including nodes (stations, switches, via points) and edges, is defined in a flexible JSON format.
-	**Flexible Output**: The calculated route can be displayed in both a detailed and a simplified JSON format.

## OHTRouteCalculator Class

The `OHTRouteCalculator` class is the core component for loading the OHT layout and calculating routes. It uses an A* search algorithm to find the shortest path between two specified nodes.

### Data Structures

-	`Node`: Represents a point in the OHT layout with an `Id`, `Type` (e.g., STATION, SWITCH, VIA_POINT), `Position` (x, y coordinates), and `SwitchOptions`.
-	`Position`: Defines the `x` and `y` coordinates of a node.
-	`Edge`: Represents a connection between two nodes with `From` and `To` node IDs, `Cost`, and `Side`.
-	`Layout`: Contains lists of `Node` and `Edge` objects, defining the entire OHT system's topology.
-	`RouteStep`: Represents a single step in the calculated route, including `No` (step number), `Type`, `Id` (node ID), `Side`, and `SwitchOption`.
-	`SimpleRouteStep`: A simplified version of `RouteStep`, containing only `No`, `Id`, and `Side`.

### Methods

#### `LoadLayout(string jsonLayout)`

Loads the OHT system layout from a JSON string. This method must be called before attempting to calculate any routes.

```csharp
string jsonLayout = "{\"nodes\": [...], \"edges\": [...]}"; // Your OHT layout JSON
OHTRouteCalculator calculator = new OHTRouteCalculator();
calculator.LoadLayout(jsonLayout);
```

#### `RouteOHT(string startNodeId, string endNodeId, string layoutName)`

Calculates the shortest route between a start node and an end node, returning a list of `RouteStep` objects. This method provides a detailed route with node types and switch options.

```csharp
List<RouteStep> route = calculator.RouteOHT("H1", "H2", "main_layout");
foreach (var step in route)
{
    Console.WriteLine($"Step {step.No}: {step.Id} (Type: {step.Type}, Side: {step.Side})");
}
```

#### `RouteOHTSimple(string startNodeId, string endNodeId, string layoutName)`

Calculates the shortest route between a start node and an end node, returning a list of `SimpleRouteStep` objects. This method provides a simplified route with only step number, node ID, and side.

```csharp
List<SimpleRouteStep> simpleRoute = calculator.RouteOHTSimple("H1", "H2", "main_layout");
foreach (var step in simpleRoute)
{
    Console.WriteLine($"Step {step.No}: {step.Id} (Side: {step.Side})");
}
```

## Code Structure

-	`OHTRouteCalculator.cs`: Contains the core logic for the A* algorithm and route calculation.
-	`CompleteTest.cs`: Provides an interactive console for testing the route calculator and defines the sample layout.
-	`OHTAlgorithm.csproj`: The C# project file.
