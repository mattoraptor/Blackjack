using System;

namespace Blackjack
{
    public class Hand
    {
        public readonly Tuple<int, int> Value;

        private Hand(Tuple<int, int> value)
        {
            Value = value;
        }

        public Hand(int a, int b) : this(new Tuple<int, int>(a, b))
        {
        }

        public int GetThePoints(int newCard)
        {
            var newCardValue = GetCardValue(newCard);
            var firstCardValue = GetCardValue(Value.Item1);
            var secondCardValue = GetCardValue(Value.Item2);
            var one = 1;
            var eleven = 11;

            var pointsIfAceIsOne = (firstCardValue == 1 ? one : firstCardValue)
                                   + (secondCardValue == 1 ? one : secondCardValue)
                                   + (newCard == 1 ? one : newCardValue);
            var pointsIfAceIsEleven = (firstCardValue == 1 ? eleven : firstCardValue)
                                      + (secondCardValue == 1 ? eleven : secondCardValue)
                                      + (newCard == 1 ? eleven : newCardValue);

            if (pointsIfAceIsEleven <= 21)
                return pointsIfAceIsEleven;
            return pointsIfAceIsOne;
        }

        public static int GetCardValue(int card)
        {
            if (card > 10)
                return 10;
            return card;
        }
    }
}