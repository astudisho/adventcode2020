using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdventCode.Exercises
{
    public static class Day22
    {
        public static string Data { get; set; }

        public static List<Queue<int>> ParseCards(string data = default)
        {
            if (string.IsNullOrEmpty(Data)) Data = InputFetcher.FetchInput("Day22.txt");

            var cards = Data.Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                            .Select
                            (x => new Queue<int>
                                (
                                    x.Split(":", StringSplitOptions.RemoveEmptyEntries)
                                    .ElementAt(1)
                                    .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => int.Parse(x))
                                )
                            )
                            .ToList();

            return cards;
        }

        public static int Exercise1()
        {
            var result = 0;

            var cards = ParseCards();

            var player1Cards = cards.First();
            var player2Cards = cards.Last();
            var round = 0;

            while (player1Cards.Any() && player2Cards.Any())
            {
                var p1Card = player1Cards.Dequeue();
                var p2Card = player2Cards.Dequeue();

                var roundWinner = p1Card > p2Card ? player1Cards : player2Cards;

                if(p1Card > p2Card)
                {
                    roundWinner.Enqueue(p1Card);
                    roundWinner.Enqueue(p2Card);
                }
                else
                {
                    roundWinner.Enqueue(p2Card);
                    roundWinner.Enqueue(p1Card);
                }
                round++;
            }

            var winner = player1Cards.Any() ? player1Cards : player2Cards;

            var winnerCards = winner.ToList().Reverse<int>().ToList();
            
            for (int i = 1; i <= winnerCards.Count(); i++)
            {
                result += winnerCards[i - 1] * i;
            }

            return result;
        }
    }
}
