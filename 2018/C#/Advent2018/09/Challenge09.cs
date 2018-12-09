using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2018._09
{
    public static class Challenge09
    {
        public static void Run()
        {
            // Parse input
            string input = "473 players; last marble is worth 70904 points";
            int players = int.Parse(input.Split(' ')[0]);
            int maxMarble = int.Parse(input.Split(' ')[6]);

            // Task 1
            ulong maxScore = MaxScore(players, maxMarble);

            Console.WriteLine(maxScore);

            // Task 2
            maxScore = MaxScore(players, maxMarble * 100);

            Console.WriteLine(maxScore);
        }


        private static ulong MaxScore(int players, int maxMarble)
        {
            // Storage for score calculation
            Dictionary<int, ulong> score = Enumerable.Range(0, players).ToDictionary(x => x, _ => (ulong)0);

            // Storage for marbles with initial marble
            LinkedList<int> marbles = new LinkedList<int>();
            LinkedListNode<int> currentNode = marbles.AddFirst(0);

            // Start the game
            for (int player = 0, marble = 1; marble <= maxMarble; ++marble, player = ++player % players)
                if (marble % 23 != 0)
                {
                    currentNode = currentNode.Next(2);

                    currentNode = marbles.AddBefore(currentNode, marble);
                }
                else
                {
                    // Add score
                    score[player] += (ulong)marble;

                    // Add score for marble 7 steps back
                    currentNode = currentNode.Previous(7);
                    score[player] += (ulong)currentNode.Value;

                    // Remove current marble
                    currentNode = currentNode.Next(1);
                    marbles.Remove(currentNode.Previous);
                }

            return score.Max(x => (ulong)x.Value);
        }
    }
}
