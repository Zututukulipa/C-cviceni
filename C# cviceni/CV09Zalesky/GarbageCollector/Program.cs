using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GarbageCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            Edge edge;
            GraphNode node;
            List<Edge> edges = new List<Edge>();
            List<GraphNode> nodes = new List<GraphNode>();
            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader("vstup.dat");
            Console.WriteLine("Loading File..");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("edge"))
                {
                    edge = new Edge();
                    while (!line.Contains("source"))
                        line = file.ReadLine();
                    if (line.Contains("source"))
                    {
                        edge.Source = nodes.First(x => x.ID == int.Parse(Regex.Match(line, @"\d+").Value));
                    }
                    line = file.ReadLine();
                    if (line.Contains("target"))
                    {
                        edge.Target = nodes.First(x => x.ID == int.Parse(Regex.Match(line, @"\d+").Value));
                    }
                    edges.Add(edge);
                }
                else if (line.Contains("node"))
                {
                    node = new GraphNode();
                    while (!line.Contains("id"))
                        line = file.ReadLine();
                    if (line.Contains("id"))
                    {
                        node.ID = int.Parse(Regex.Match(line, @"\d+").Value);
                    }
                    line = file.ReadLine();
                    if (line.Contains("label"))
                    {
                        if (line.Contains("ROOT"))
                            node.Label = "ROOT";
                        node.Label = Regex.Match(line, @"\d+").ToString();
                    }
                    nodes.Add(node);
                }

            }
            file.Close();
            Console.WriteLine("File Loaded!");
            removeUnconnectedEdges(edges);
            var roots = nodes.Where(p => edges.All(p2 => p2.Target != p)).ToList();
            Console.Write($"Found {roots.Count} roots.\nRoots are:\n");
            for (int i = 0; i < roots.Count; ++i)
            {
                Console.Write($"[{i}] ID: {roots[i].ID}\n");
            }

            for (int i = 0; i < roots.Count; ++i)
            {
                GraphNode root = roots[i];
                var connectedPath = getGraphPathFromSource(edges, root);
                Console.WriteLine($"\nPath of [{i}] with Length {connectedPath.Count}\nVizualize? (Y - Yes, Other - No)");
                var output = Console.ReadLine();
                if (output == "Y" || output == "y")
                {
                    if (connectedPath.Count == 0)
                        Console.Write($"({root.ID})");
                    else
                    {
                        for (int j = 0; j < connectedPath.Count; j++)
                        {
                            var segment = connectedPath[j];
                            if (j > 0 && segment.Source != connectedPath[j - 1].Source)
                                Console.Write($"\n[{segment.Source.ID}]\n");
                            Console.Write($"({segment.Source.ID}) -> ({segment.Target.ID})  ");
                        }
                    }
                    Console.WriteLine($"\nUnconnected nodes count from {root.Label} = {edges.Count - connectedPath.Count}");
                }

            }
        }

        public static List<Edge> locatePath()
        {
            return null;
        }
        public static List<Edge> getPaths(List<Edge> currentKnownPath, List<Edge> graph)
        {
            List<Edge> newPaths = new List<Edge>();
            foreach (var item in currentKnownPath)
            {
                newPaths.AddRange(getPathsToConnectedNodes(graph, item.Target));
            }

            currentKnownPath.AddRange(newPaths);
            return currentKnownPath;
        }

        public static List<Edge> getPathsToConnectedNodes(List<Edge> nodes, GraphNode source)
        {
            List<Edge> connectedNodes;
            connectedNodes = nodes.FindAll(x => x.Source.ID == source.ID).ToList();
            foreach (var node in connectedNodes)
            {
                if (node.Target.Equals(node.Source))
                    connectedNodes.Remove(node);
            }


            return connectedNodes;
        }

        public static List<Edge> getGraphPathFromSource(List<Edge> graph, GraphNode source)
        {
            List<Edge> finalPath = new List<Edge>();
            finalPath.AddRange(getPathsToConnectedNodes(graph, source));
            if (finalPath.Count == 0)
            {
                Console.WriteLine($"Unconnected node: {source.ID}");
                return new List<Edge>();
            }
            int end = 1;
            for (int i = 0; i < end; ++i)
            {
                var path = getPathsToConnectedNodes(graph, finalPath[i].Target);
                finalPath.AddRange(path.Where(x => !finalPath.Any(y => y == x)));
                end = finalPath.Count;
            }
            return finalPath;
        }

        public static void removeUnconnectedEdges(List<Edge> edges)
        {
            var disconnectedNodes = edges.FindAll(x => x.Source == x.Target).Select(x => x).ToList();
            foreach (var item in disconnectedNodes)
            {
                edges.Remove(item);
            }
        }
    }
}
