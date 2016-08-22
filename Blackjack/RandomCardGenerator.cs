using System;

namespace Blackjack
{
    internal class RandomCardGenerator : ICardGenerator
    {
        private readonly Random _random;

        public RandomCardGenerator()
        {
            _random = new Random();
        }

        public int NextCard()
        {
            return _random.Next(1, 14);
        }
    }
}