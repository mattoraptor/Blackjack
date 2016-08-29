using System.Collections.Generic;
using System.Linq;
using Blackjack;

namespace BlackjackTests
{
    public class TestCardGenerator : ICardGenerator
    {
        private readonly Queue<int> _cards = new Queue<int>();
        public int Card = 10;

        public int NextCard()
        {
            if (_cards.Any())
                return NextCardFromList();
            return Card;
        }

        public void AddCards(params int[] cards)
        {
            foreach (var card in cards)
            {
                _cards.Enqueue(card);
            }
        }

        private int NextCardFromList()
        {
            return _cards.Dequeue();
        }
    }
}