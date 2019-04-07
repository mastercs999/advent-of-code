using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2016._10
{
    public static class Challenge10
    {
        public static void Run()
        {
            // Load bots
            (List<Bot> bots, Dictionary<int, OutputBin> outputBins) = Load(Path.Combine("10", "input"));

            // Task 1
            int botId = 0;
            while (bots.Any(x => x.HasChip))
                foreach (Bot bot in bots)
                {
                    // Task 1 check
                    if (bot.HasBothChips && bot.Chips.Select(x => x.Value).ToHashSet().SetEquals(new HashSet<int>() { 61, 17 }))
                        botId = bot.Id;

                    bot.Action();
                }

            Console.WriteLine(botId);

            // Task 2
            int suma = new OutputBin[] { outputBins[0], outputBins[1], outputBins[2] }.SelectMany(x => x.Chips).Aggregate((a, b) => a * b);

            Console.WriteLine(suma);
        }

        public static (List<Bot>, Dictionary<int, OutputBin>) Load(string path)
        {
            Dictionary<int, Bot> bots = new Dictionary<int, Bot>();
            Dictionary<int, OutputBin> outputBins = new Dictionary<int, OutputBin>();

            Bot getBot(int botId)
            {
                if (bots.TryGetValue(botId, out Bot bot))
                    return bot;
                else
                    return bots[botId] = new Bot(botId);
            }
            OutputBin getBin(int binId)
            {
                if (outputBins.TryGetValue(binId, out OutputBin bin))
                    return bin;
                else
                    return outputBins[binId] = new OutputBin(binId);
            }

            foreach (string line in File.ReadAllLines(path))
            {
                Match load = Regex.Match(line, @"value (\d+) goes to bot (\d+)");
                Match send = Regex.Match(line, @"bot (\d+) gives low to (output|bot) (\d+) and high to (output|bot) (\d+)");

                if (load.Success)
                {
                    // Load line data
                    int botId = int.Parse(load.Groups[2].Value);
                    int chip = int.Parse(load.Groups[1].Value);

                    getBot(botId).Receive(chip);
                }
                else
                {
                    // Load line data
                    int botId = int.Parse(send.Groups[1].Value);
                    List<(string target, int id)> targets = new List<(string target, int id)>()
                    {
                        (send.Groups[2].Value, int.Parse(send.Groups[3].Value)),
                        (send.Groups[4].Value, int.Parse(send.Groups[5].Value)),
                    };

                    // Define the action
                    Bot bot = getBot(botId);
                    bot.Action = new Action(() =>
                    {
                        if (!bot.HasBothChips || !targets.Where(x => x.target == "bot").All(x => getBot(x.id).CanAccept))
                            return;
                        
                        // Withdraw chips from bot
                        int[] chips = new int[]
                        {
                            bot.Withdraw(bot.LowerChipIndex),
                            bot.Withdraw(bot.HigherChipIndex)
                        };

                        // Send chips to another bots
                        for (int i = 0; i < 2; ++i)
                            if (targets[i].target == "bot")
                                getBot(targets[i].id).Receive(chips[i]);
                            else
                                getBin(targets[i].id).Receive(chips[i]);
                    });
                }
            };
            
            return (bots.Select(x => x.Value).ToList(), outputBins);
        }
    }
}
