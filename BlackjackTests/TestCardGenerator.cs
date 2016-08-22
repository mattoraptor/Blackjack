using Blackjack;

namespace BlackjackTests
{
    public class TestCardGenerator : ICardGenerator
    {
        public int NextCard()
        {
            return 10;
        }
    }
}