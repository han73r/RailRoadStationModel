namespace Structure;

// 1. Придумать и реализовать структуру данных, которая будет содержать в себе отрезки и точки (схема станции).
public class Vertex
{
    public char Id { get; }
    private static char _lastId = 'a';
    public double X { get; }
    public double Y { get; }
    public Vertex(double x, double y) {
        Id = _lastId++;
        X = x;
        Y = y;
    }
    public override string ToString() {
        return $"({Id}: {X}, {Y})";
    }
}
public class Edge
{
    public int Id { get; }
    public static int _lastId = 1;
    public Vertex From { get; }
    public Vertex To { get; }
    public double Weight { get; }
    public Edge(Vertex from, Vertex to) {
        Id = _lastId++;
        From = from;
        To = to;
        Weight = ComputeWeight();
    }
    private double ComputeWeight() {
        return Math.Sqrt(Math.Pow(To.X - From.X, 2) + Math.Pow(To.Y - From.Y, 2));
    }
    public override string ToString() {
        return $"Id: {Id}: {From} -> {To} : W:{Weight}";
    }
}
public class RailRoadPath
{
    public string Name { get; }
    private List<Vertex> vertices;
    private List<Edge> edges;
    private Dictionary<Vertex, List<Edge>> adjList;
    public RailRoadPath(string name) {
        Name = name;
        vertices = new List<Vertex>();
        edges = new List<Edge>();
        adjList = new Dictionary<Vertex, List<Edge>>();
    }
    public virtual void AddEdge(Vertex from, Vertex to) {
        EnsureVertexExists(from);
        EnsureVertexExists(to);
        var edge = new Edge(from, to);
        edges.Add(edge);
        adjList[from].Add(edge);
    }
    public List<Vertex> GetVertices() { return vertices; }
    public List<Edge> GetEdges() { return edges; }
    public (Vertex startVertex, Vertex endVertex) ChooseVerticesForShortestPath(Edge startEdge, Edge endEdge) {
        Vertex startVertex = startEdge.From;
        Vertex endVertex = endEdge.To;
        return (startVertex, endVertex);
    }
    public (double shortestPathWeight, List<Edge> shortestPathEdges) FindEdgesBetweenVertices(Vertex startVertex, Vertex endVertex) {
        // Минимальный вес пути между startVertex и endVertex
        double shortestPathWeight = double.PositiveInfinity;
        // Список для хранения кратчайшего пути
        List<Edge> shortestPathEdges = new List<Edge>();
        // Стек для обхода в глубину
        Stack<(Vertex vertex, List<Edge> pathEdges, double pathWeight)> stack = new Stack<(Vertex, List<Edge>, double)>();
        stack.Push((startVertex, new List<Edge>(), 0));
        // Множество посещённых вершин
        HashSet<Vertex> visited = new HashSet<Vertex>();
        visited.Add(startVertex);
        bool pathFound = false;
        while (stack.Count > 0) {
            var (currentVertex, currentPathEdges, currentPathWeight) = stack.Pop();
            // Перебираем все рёбра исходящие из текущей вершины
            foreach (var edge in adjList[currentVertex]) {
                Vertex neighbor = edge.To;
                // Если соседняя вершина ещё не была посещена
                if (!visited.Contains(neighbor)) {
                    // Создаём новый путь, добавляя текущее ребро к пути
                    var newPathEdges = new List<Edge>(currentPathEdges);
                    newPathEdges.Add(edge);
                    double newPathWeight = currentPathWeight + edge.Weight;
                    visited.Add(neighbor);
                    stack.Push((neighbor, newPathEdges, newPathWeight));
                    // Если достигли конечной вершины
                    if (neighbor == endVertex) {
                        // Проверяем, является ли найденный путь кратчайшим
                        if (newPathWeight < shortestPathWeight) {
                            shortestPathWeight = newPathWeight;
                            shortestPathEdges = newPathEdges;
                            pathFound = true;
                        }
                    }
                }
            }
        }
        if (!pathFound) {
            return (double.PositiveInfinity, new List<Edge>());
        }
        return (shortestPathWeight, shortestPathEdges);
    }
    private void EnsureVertexExists(Vertex vertex) {
        if (!vertices.Contains(vertex)) {
            vertices.Add(vertex);
            adjList[vertex] = new List<Edge>();
        }
    }
    public class Park : RailRoadPath
    {
        public int Id { get; }
        private static int _lastId = 0;
        private List<RailRoadPath> railRoadPaths;
        private Lazy<List<Vertex>> lazyConvexHull;
        public List<Vertex> ConvexHull => lazyConvexHull.Value;
        public Park(string name) : base(name) {
            Id = _lastId++;
            railRoadPaths = new List<RailRoadPath>();
            lazyConvexHull = new Lazy<List<Vertex>>(ComputeConvexHull);
        }
        public void AddGraph(RailRoadPath graph) {
            if (railRoadPaths.Contains(graph)) { return; }
            railRoadPaths.Add(graph);
            // Для каждой вершины входного графа проверяем, есть ли она уже в списке vertices
            // и добавляем только уникальные вершины
            foreach (var vertex in graph.GetVertices()) {
                if (!vertices.Contains(vertex)) {
                    vertices.Add(vertex);
                    adjList[vertex] = new List<Edge>();
                }
            }
            // Добавляем все рёбра в общий список edges
            edges.AddRange(graph.GetEdges());
            foreach (var edge in graph.GetEdges()) {
                // Проверяем, существует ли уже такое ребро в adjList
                bool edgeExists = adjList.ContainsKey(edge.From) && adjList[edge.From].Any(e => e.To == edge.To);
                if (!edgeExists) {
                    // Добавляем ребро от From к To
                    adjList[edge.From].Add(edge);
                    // Если граф ненаправленный и не совпадает с обратным ребром, добавляем обратное ребро
                    if (edge.From != edge.To) {
                        bool reverseEdgeExists = adjList.ContainsKey(edge.To) && adjList[edge.To].Any(e => e.To == edge.From);
                        if (!reverseEdgeExists) {
                            adjList[edge.To].Add(new Edge(edge.To, edge.From));
                        }
                    }
                }
            }
        }
        public List<RailRoadPath> GetGraphs() { return railRoadPaths; }
        public List<Vertex> ComputeConvexHull() {
            if (vertices.Count < 3)
                throw new ArgumentException("At least 3 vertices are required");
            List<Vertex> hull = new List<Vertex>();
            // Находим самую левую точку
            Vertex leftmost = vertices[0];
            foreach (var vertex in vertices) {
                if (vertex.X < leftmost.X) {
                    leftmost = vertex;
                }
            }
            Vertex p = leftmost;
            do {
                hull.Add(p);
                Vertex q = vertices[0];
                foreach (var r in vertices) {
                    if (Orientation(p, q, r) == 2 || (Orientation(p, q, r) == 0 && Distance(p, r) > Distance(p, q))) {
                        q = r;
                    }
                }
                p = q;
            } while (p != leftmost);
            var minIdVertex = hull.MinBy(v => v.Id);
            int minIdIndex = hull.IndexOf(minIdVertex);
            // Циклический сдвиг массива начиная с вершины с наименьшим Id
            var rotatedHull = hull.Skip(minIdIndex).Concat(hull.Take(minIdIndex)).ToList();
            return rotatedHull;
        }
        private int Orientation(Vertex p, Vertex q, Vertex r) {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0)
                return 0; // коллинеарные
            return (val > 0) ? 1 : 2; // часовая стрелка или против часовой стрелки
        }
        private double Distance(Vertex p, Vertex q) {
            return Math.Sqrt(Math.Pow(p.X - q.X, 2) + Math.Pow(p.Y - q.Y, 2));
        }
        public void PrintAllRailRoadPaths() {
            Console.WriteLine($"Доступные пути парка {Name}:");
            railRoadPaths.ForEach(e => Console.WriteLine(e.Name));
        }
        public void PrintAllRailEdges() {
            Console.WriteLine($"Доступные участки путей парка {Name}:");
            railRoadPaths.SelectMany(railRoad => railRoad.GetEdges())
                         .ToList()
                         .ForEach(Console.WriteLine);
        }
        public void PrintConvexHull() {
            Console.WriteLine("Выпуклая оболочка включает в себя следующие точки:");
            foreach (var item in ConvexHull) {
                Console.WriteLine(item);
            }
        }
    }
}