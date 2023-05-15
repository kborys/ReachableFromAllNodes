internal class Graph
{
    public int V; // number of vertices
    public List<List<int>> adj; // adjacency list

    public Graph(int v)
    {
        V = v;
        adj = new List<List<int>>();
        for (var i = 0; i < V; i++) adj.Add(new List<int>());
    }

    public void AddEdge(int u, int v)
    {
        adj[u].Add(v);
    }

    public List<List<int>> ReverseGraph()
    {
        var rev = new List<List<int>>();
        for (var i = 0; i < V; i++) rev.Add(new List<int>());
        for (var u = 0; u < V; u++)
            foreach (var v in adj[u])
                rev[v].Add(u);
        return rev;
    }

    public void DFS(int v, bool[] visited, Action<int> pre, Action<int> post)
    {
        visited[v] = true;
        pre(v);
        foreach (var u in adj[v])
            if (!visited[u])
                DFS(u, visited, pre, post);
        post(v);
    }
    public static Graph ParseDotToGraph(string[] strings)
    {
        var edges = new List<Tuple<int, int>>();

        foreach (var line in strings)
        {
            if (!line.Contains("->")) continue;

            var parts = line.Split("->");
            var source = int.Parse(parts[0].Trim());
            var destination = int.Parse(parts[1].Trim().TrimEnd(';'));

            edges.Add(Tuple.Create(source, destination));
        }

        var numVertices = edges.Select(t => t.Item1).Union(edges.Select(t => t.Item2)).Distinct().Count();
        var graph = new Graph(numVertices);
        foreach (var edge in edges) graph.AddEdge(edge.Item1, edge.Item2);

        return graph;
    }
}