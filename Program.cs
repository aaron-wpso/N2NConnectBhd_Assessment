using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize deck of cards
            string[] suits = { "@", "#", "^", "*" };
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            List<string> deck = new List<string>();

            foreach (var value in values)
            {
                foreach (var suit in suits)
                {
                    deck.Add(value + suit);
                }
            }

            // Shuffle the deck
            Random random = new Random();
            deck = deck.OrderBy(x => random.Next()).ToList();

            // Distribute the cards to 4 players
            Dictionary<int, List<string>> players = new Dictionary<int, List<string>>
            {
                { 1, new List<string>() },
                { 2, new List<string>() },
                { 3, new List<string>() },
                { 4, new List<string>() }
            };

            for (int i = 0; i < deck.Count; i++)
            {
                players[(i % 4) + 1].Add(deck[i]);
            }

            // Display the cards of each player
            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine($"Player {i}: {string.Join(", ", players[i])}");
            }

            // Evaluate each player's hand
            Dictionary<int, Dictionary<string, int>> playerEvaluations = new Dictionary<int, Dictionary<string, int>>();

            foreach (var player in players)
            {
                Dictionary<string, int> valueCounts = new Dictionary<string, int>();
                foreach (var card in player.Value)
                {
                    string value = card.Substring(0, card.Length - 1);
                    if (valueCounts.ContainsKey(value))
                    {
                        valueCounts[value]++;
                    }
                    else
                    {
                        valueCounts[value] = 1;
                    }
                }
                playerEvaluations[player.Key] = valueCounts;
            }

            // Determine the winner
            int bestValueCount = 0;
            string bestValue = "";
            int bestPlayer = 0;

            foreach (var player in playerEvaluations)
            {
                int maxCount = player.Value.Values.Max();
                string bestHandValue = player.Value.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

                if (maxCount > bestValueCount)
                {
                    bestValueCount = maxCount;
                    bestValue = bestHandValue;
                    bestPlayer = player.Key;
                }
                else if (maxCount == bestValueCount)
                {
                    if (Array.IndexOf(values, bestHandValue) > Array.IndexOf(values, bestValue))
                    {
                        bestValue = bestHandValue;
                        bestPlayer = player.Key;
                    }
                }
            }

            // Display the winner
            Console.WriteLine($"\nThe winner is Player {bestPlayer} with {bestValueCount} cards of {bestValue}.");
        }
    }
}
