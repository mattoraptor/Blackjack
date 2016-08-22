using Blackjack;

namespace BlackjackTests
{
    public class TestCardGenerator : ICardGenerator
    {
        public int Card { get; set; } = 10;

        public int NextCard()
        {
            return Card;
        }
    }
}