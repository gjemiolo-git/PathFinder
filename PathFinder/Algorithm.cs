// Option A
// 40506637
// Gregory Jemiolo

public interface DijkstraAlgorithm
{
    public struct Vertex
    {
        public int x = 0;
        public int y = 0;

        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vertex(string x, string y)
        {
            this.x = Int32.Parse(x);
            this.y = Int32.Parse(y);
        }

        public override string ToString()
        {
            string v = "(" + this.x + "," + this.y + ")";
            return v;
        }
    }

    public static double GetEuclidanDistance(Vertex a, Vertex b)
    {
        double xResult = Math.Pow(b.x - a.x, 2);
        double yResult = Math.Pow(b.y - a.y, 2);

        return Math.Sqrt(xResult + yResult);
    }

    // C# Implementation of Dijkstra's algorithm
    public static List<int> GetOptimalPath(double[,] graph, int src, int dest)
    {
        int vertexCount = graph.GetLength(0);
        double[] distance = new double[vertexCount];
        bool[] shortestPath = new bool[vertexCount];
        int[] previous = new int[vertexCount];          // previous node in optimal path

        // Initialise values of distance and shortestPath
        for (int i = 0; i < vertexCount; i++)
        {
            distance[i] = double.MaxValue;
            shortestPath[i] = false;
        }
        distance[src] = 0;

        for (int count = 0; count < vertexCount - 1; count++)
        {
            int u = SelectClosestVertex(distance, shortestPath);
            shortestPath[u] = true;

            if (u == dest) break; // Destination vertex is reached

            for (int v = 0; v < vertexCount; v++)
            {
                if (!shortestPath[v]             // Check if node 
                    && Convert.ToBoolean(graph[v, u])
                    && distance[u] != double.MaxValue
                    && distance[u] + graph[v, u] < distance[v])
                {
                    previous[v] = u;
                    distance[v] = distance[u] + graph[v, u];
                }
            }
        }

        // Build the shortest path using the 'previous' array
        List<int> path = new List<int>();
        int step = dest;                    // Follow path back to the source
        double testDist = Math.Round(distance[step], 2);

        if (distance[step] == double.MaxValue)
        {
            // If the destination is unreachable, clear the path and return an empty path.
            path.Clear();
            return path;
        }

        while (step != src)
        {
            path.Insert(0, step);
            step = previous[step];
        }
        path.Insert(0, src);

        //Console.WriteLine(testDist);
        return path;
    }
    private static int SelectClosestVertex(double[] distance, bool[] shortestPathTreeSet)
    {
        double min = double.MaxValue;   // Setting min value
        int minIndex = -1;              // Setting dummy index

        for (int v = 0; v < distance.Length; v++)                           // Looping over vertices
            if (shortestPathTreeSet[v] == false && distance[v] <= min)      // If vertex v is the shortest, and is not included in the path then update the current shorteset
            {
                min = distance[v];
                minIndex = v;
            }

        return minIndex;    // Return index of the new closest verterx
    }
}