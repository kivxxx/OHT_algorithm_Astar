using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OHTAlgorithm
{
    public class Node
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        
        [JsonProperty("position")]
        public Position Position { get; set; } = new Position();
        
        [JsonProperty("switch_options")]
        public Dictionary<string, string> SwitchOptions { get; set; } = new Dictionary<string, string>();
    }

    public class Position
    {
        [JsonProperty("x")]
        public int X { get; set; }
        
        [JsonProperty("y")]
        public int Y { get; set; }
    }

    public class Edge
    {
        [JsonProperty("from")]
        public string From { get; set; } = string.Empty;
        
        [JsonProperty("to")]
        public string To { get; set; } = string.Empty;
        
        [JsonProperty("cost")]
        public int Cost { get; set; }
        
        [JsonProperty("side")]
        public string Side { get; set; } = string.Empty;
    }

    public class Layout
    {
        [JsonProperty("nodes")]
        public List<Node> Nodes { get; set; } = new List<Node>();
        
        [JsonProperty("edges")]
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }

    public class RouteStep
    {
        [JsonProperty("no")]
        public int No { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonProperty("side")]
        public string Side { get; set; } = string.Empty;
        
        [JsonProperty("switch-option")]
        public string SwitchOption { get; set; } = string.Empty;
    }

    public class SimpleRouteStep
    {
        [JsonProperty("no")]
        public int No { get; set; }
        
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonProperty("side")]
        public string Side { get; set; } = string.Empty;
    }

    public class OHTRouteCalculator
    {
        private Layout? _layout;
        private Dictionary<string, Node> _nodeDict = new Dictionary<string, Node>();
        private Dictionary<string, List<Edge>> _adjacencyList = new Dictionary<string, List<Edge>>();

        public void LoadLayout(string jsonLayout)
        {
            _layout = JsonConvert.DeserializeObject<Layout>(jsonLayout);
            if (_layout?.Nodes != null)
            {
                _nodeDict = _layout.Nodes.ToDictionary(n => n.Id, n => n);
                BuildAdjacencyList();
            }
        }

        private void BuildAdjacencyList()
        {
            if (_layout?.Nodes == null || _layout?.Edges == null) return;
            
            _adjacencyList = new Dictionary<string, List<Edge>>();
            
            foreach (var node in _layout.Nodes)
            {
                _adjacencyList[node.Id] = new List<Edge>();
            }

            foreach (var edge in _layout.Edges)
            {
                _adjacencyList[edge.From].Add(edge);
                
                // 添加反向邊（假設軌道是雙向的）
                var reverseEdge = new Edge
                {
                    From = edge.To,
                    To = edge.From,
                    Cost = edge.Cost,
                    Side = edge.Side == "left" ? "right" : "left"
                };
                _adjacencyList[edge.To].Add(reverseEdge);
            }
        }

        public List<RouteStep> RouteOHT(string startNodeId, string endNodeId, string layoutName)
        {
            return RouteOHTInternal(startNodeId, endNodeId, layoutName, false);
        }

        public List<RouteStep> RouteOHT(string startNodeId, string endNodeId, string layoutName, string format)
        {
            bool isSimple = format?.ToLower() == "simple";
            return RouteOHTInternal(startNodeId, endNodeId, layoutName, isSimple);
        }

        public List<SimpleRouteStep> RouteOHTSimple(string startNodeId, string endNodeId, string layoutName)
        {
            var fullResult = RouteOHTInternal(startNodeId, endNodeId, layoutName, true);
            return fullResult.Select(step => new SimpleRouteStep
            {
                No = step.No,
                Id = step.Id,
                Side = step.Side
            }).ToList();
        }

        private List<RouteStep> RouteOHTInternal(string startNodeId, string endNodeId, string layoutName, bool isSimpleFormat)
        {
            if (_layout == null)
            {
                throw new InvalidOperationException("Layout not loaded. Call LoadLayout first.");
            }

            if (!_nodeDict.ContainsKey(startNodeId))
            {
                throw new ArgumentException($"Start node '{startNodeId}' not found in layout.");
            }

            if (!_nodeDict.ContainsKey(endNodeId))
            {
                throw new ArgumentException($"End node '{endNodeId}' not found in layout.");
            }

            // 處理同節點的情況
            if (startNodeId == endNodeId)
            {
                var sameNodePath = new List<PathNode>
                {
                    new PathNode { NodeId = startNodeId, Edge = null }
                };
                return ConvertPathToRouteSteps(sameNodePath, isSimpleFormat);
            }

            var path = FindShortestPath(startNodeId, endNodeId);
            
            if (path == null || path.Count == 0)
            {
                throw new InvalidOperationException($"No route found from '{startNodeId}' to '{endNodeId}'.");
            }

            return ConvertPathToRouteSteps(path, isSimpleFormat);
        }

        private List<PathNode>? FindShortestPath(string startId, string endId)
        {
            // A* 演算法實現
            var gScore = new Dictionary<string, double>(); // 從起點到當前節點的實際代價
            var fScore = new Dictionary<string, double>(); // 總估計代價 f = g + h
            var previous = new Dictionary<string, PathNode>();
            var openSet = new List<string>(); // 待處理的節點
            var closedSet = new HashSet<string>(); // 已處理的節點

            // 初始化所有節點的 g 和 f 值
            foreach (var nodeId in _nodeDict.Keys)
            {
                gScore[nodeId] = double.MaxValue;
                fScore[nodeId] = double.MaxValue;
            }

            // 起始節點
            gScore[startId] = 0;
            fScore[startId] = CalculateHeuristic(startId, endId);
            openSet.Add(startId);

            while (openSet.Count > 0)
            {
                // 找到 f 值最小的節點
                var currentId = openSet.OrderBy(id => fScore[id]).First();

                // 如果到達目標節點，結束搜索
                if (currentId == endId)
                {
                    return ReconstructPath(previous, startId, endId);
                }

                openSet.Remove(currentId);
                closedSet.Add(currentId);

                // 檢查所有鄰居
                if (_adjacencyList.ContainsKey(currentId))
                {
                    foreach (var edge in _adjacencyList[currentId])
                    {
                        var neighborId = edge.To;

                        // 如果鄰居已經在閉集中，跳過
                        if (closedSet.Contains(neighborId))
                            continue;

                        // 計算從起點經過當前節點到鄰居的暫定 g 值
                        var tentativeGScore = gScore[currentId] + edge.Cost;

                        // 如果鄰居不在開集中，加入開集
                        if (!openSet.Contains(neighborId))
                        {
                            openSet.Add(neighborId);
                        }
                        else if (tentativeGScore >= gScore[neighborId])
                        {
                            // 如果新路徑不比已知路徑更好，跳過
                            continue;
                        }

                        // 這條路徑是到鄰居的最佳路徑
                        previous[neighborId] = new PathNode 
                        { 
                            NodeId = currentId, 
                            Edge = edge 
                        };
                        gScore[neighborId] = tentativeGScore;
                        fScore[neighborId] = gScore[neighborId] + CalculateHeuristic(neighborId, endId);
                    }
                }
            }

            // 沒有找到路徑
            return null;
        }

        private double CalculateHeuristic(string fromId, string toId)
        {
            // 使用歐幾里得距離作為啟發式函數
            var fromNode = _nodeDict[fromId];
            var toNode = _nodeDict[toId];
            
            var dx = fromNode.Position.X - toNode.Position.X;
            var dy = fromNode.Position.Y - toNode.Position.Y;
            
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private List<PathNode>? ReconstructPath(Dictionary<string, PathNode> previous, string startId, string endId)
        {
            if (!previous.ContainsKey(endId))
                return null;

            var path = new List<PathNode>();
            var current = endId;
            
            while (current != startId)
            {
                var pathNode = previous[current];
                path.Insert(0, new PathNode { NodeId = current, Edge = pathNode.Edge });
                current = pathNode.NodeId;
            }
            
            // 添加起始節點
            path.Insert(0, new PathNode { NodeId = startId, Edge = null });

            return path;
        }

        private List<RouteStep> ConvertPathToRouteSteps(List<PathNode> path, bool isSimpleFormat = false)
        {
            var routeSteps = new List<RouteStep>();

            for (int i = 0; i < path.Count; i++)
            {
                var pathNode = path[i];
                var node = _nodeDict[pathNode.NodeId];
                
                string side = "none";
                if (pathNode.Edge != null)
                {
                    side = pathNode.Edge.Side;
                }

                var routeStep = new RouteStep
                {
                    No = i,
                    Id = node.Id,
                    Side = side
                };

                // 根據格式參數決定是否填充額外欄位
                if (!isSimpleFormat)
                {
                    routeStep.Type = node.Type;
                    routeStep.SwitchOption = "none"; // 可以根據需要調整開關選項邏輯
                }

                routeSteps.Add(routeStep);
            }

            return routeSteps;
        }

        private class PathNode
        {
            public string NodeId { get; set; } = string.Empty;
            public Edge? Edge { get; set; }
        }
    }


} 