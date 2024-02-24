// Option A
// 40506637
// Gregory Jemiolo
class PathFinder
{
    static int source = 0;
    static int vertexCount = 0;

    static void Main(string[] args)
    {
        PathFinder shell = new PathFinder();

        if (args.Length < 1 || !File.Exists(args[0] + ".cav"))
        {
            Console.WriteLine("Please provide a valid file name.");
            return;
        }

        string fileName = args[0];
        double[,]? graph = ReadFile(fileName + ".cav");
        List<int> path = DijkstraAlgorithm.GetOptimalPath(graph, source, vertexCount - 1);

        if (path.Count == 0)
        {
            string line = "0";
            File.WriteAllText($"{fileName}.csn", line);
            //Console.WriteLine("0");
        }
        else if (path.Count > 0)
        {
            string line = String.Join(" ", path.Select(x => x + 1));
            File.WriteAllText($"{fileName}.csn", line);
            //Console.WriteLine(line);
        }
    }

    public static double[,] ReadFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine("Specified file does not exist.");
            return new double[0, 0];
        }

        ushort[]? nums = null;
        double[,]? graph = null;

        int graphStartIndex = 0;
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        using (BufferedStream bufferedStream = new BufferedStream(fileStream))
        using (StreamReader streamReader = new StreamReader(bufferedStream))
        {
            int currentArrayIndex = 0;
            int currentNumberBeingRead = 0;
            int currentCharAsInt;

            while ((currentCharAsInt = streamReader.Read()) != -1)
            {
                if (currentCharAsInt == ',') // Comma for the next number
                {
                    if (nums == null) // If it's the first number in the file
                    {
                        vertexCount = currentNumberBeingRead;
                        graphStartIndex = 1 + vertexCount * 2;

                        nums = new ushort[graphStartIndex + vertexCount * vertexCount]; // Initialize the array based on vertexCount
                        graph = new double[vertexCount, vertexCount];
                    }
                    nums[currentArrayIndex++] = (ushort)currentNumberBeingRead; // Add the currently read number to the array
                    currentNumberBeingRead = 0; // Reset the currentNumberBeingRead to the next number
                }
                else
                {
                    // Collect digits to get a number, from R to L, subtract 48 as digits start from 48 in ASCI
                    currentNumberBeingRead = currentNumberBeingRead * 10 + (currentCharAsInt - 48);
                }
            }
            // Add the last number after finishing the loop, as it doesnt have a coma
            nums[currentArrayIndex] = (ushort)currentNumberBeingRead;
        }

        // Assign vertices for Euclidan Calculations
        DijkstraAlgorithm.Vertex[] vertices = new DijkstraAlgorithm.Vertex[vertexCount];
        for (int i = 0, j = 1; i < vertexCount; i++)
        {
            vertices[i] = new DijkstraAlgorithm.Vertex(nums[j++], nums[j++]);
        }

        // Create Graph and Assign distance
        for (int i = 0; i < vertexCount; i++)
        {
            for (int j = 0; j < vertexCount; j++, graphStartIndex++)
            {
                if (nums[graphStartIndex] == 1)
                {
                    graph[i, j] = DijkstraAlgorithm.GetEuclidanDistance(vertices[i], vertices[j]);
                }
                else
                {
                    graph[i, j] = 0d;
                }
            }
        }
        return graph;
    }
}