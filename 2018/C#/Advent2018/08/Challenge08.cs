using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._08
{
    public static class Challenge08
    {
        public static void Run()
        {
            // Load numbers
            List<int> numbers = File.ReadAllText(Path.Combine("08", "input")).Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            // Build the tree
            Node root = BuildTree(numbers, 0);

            // Task 1
            // Sum all metadatas
            int suma = AllNodes(root).SelectMany(x => x.Metadata).Sum();

            Console.WriteLine(suma);

            // Task 2
            // Get vlaue of root node
            int rootValue = root.Value;

            Console.WriteLine(rootValue);
        }

        private static Node BuildTree(List<int> numbers, int start)
        {
            int childNodesCount = numbers[start];
            int metadataCount = numbers[start + 1];

            // Get child nodes
            int shift = 2;
            List<Node> childNodes = new List<Node>(20);
            for (int i = 0; i < childNodesCount; ++i)
            {
                Node child = BuildTree(numbers, start + shift);

                shift += child.Length;
                childNodes.Add(child);
            }

            // Get metadatas
            List<int> metadata = numbers.GetRange(start + shift, metadataCount);

            // Create this node
            return new Node(metadata, childNodes);
        }
        
        private static IEnumerable<Node> AllNodes(Node node)
        {
            return new Node[] { node }.Concat(node.ChildNodes.SelectMany(x => AllNodes(x)));
        }
    }
}
