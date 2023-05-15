// Step 0: Parse the dot file to a graph
Console.WriteLine("Enter the path to the dot file with the graph: ");
var path = Console.ReadLine();
var lines = File.ReadAllLines(path);
var graph = Graph.ParseDotToGraph(lines);

// Step 1: Perform first DFS to get finishing times
var visited = new bool[graph.V];
var stack = new Stack<int>();
Action<int> pushToStack = v => stack.Push(v);
for (var v = 0; v < graph.V; v++)
    if (!visited[v])
        graph.DFS(v, visited, pushToStack, pushToStack);

// Step 2: Reverse the graph
var rev = graph.ReverseGraph();

// Step 3: Perform second DFS in the order of finishing times on reversed graph
visited = new bool[graph.V];
var sccList = new List<List<int>>();
while (stack.Count > 0)
{
    var v = stack.Pop();
    if (!visited[v])
    {
        var scc = new List<int>();
        var revGraph = new Graph(graph.V); // create a new graph object
        revGraph.adj = rev; // set its adjacency list to the reversed graph
        revGraph.DFS(v, visited, u => scc.Add(u), u => { });
        sccList.Add(scc);
    }
}

// Step 4: Identify nodes reachable from all other nodes
var reachableFromAll = new HashSet<int>();
foreach (var scc in sccList)
{
    var reachable = new HashSet<int>(scc);
    foreach (var v in scc)
    {
        if (!reachable.Contains(v)) continue; 
        var visitedFromV = new bool[graph.V];
        var graphFromV = new Graph(graph.V);
        graphFromV.adj = graph.adj; 
        graphFromV.DFS(v, visitedFromV, u => reachable.Add(u), u => { });
        if (reachable.Count == scc.Count)
        {
            reachableFromAll.UnionWith(scc);
            break; // all nodes in scc are reachable
        }
    }
}

Console.WriteLine("Nodes reachable from all other nodes:");
Console.Write("{ " + string.Join(", ", reachableFromAll.OrderBy(x => x)) + " }");