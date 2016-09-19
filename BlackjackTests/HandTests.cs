using Blackjack;
using NUnit.Framework;

namespace BlackjackTests
{
    [TestFixture]
    public class HandTests
    {
        [Test]
        [Ignore("WIP two aces")]
        public void HandPoints_TwoAces_UsesFirstAceAsEleven_AndSecondAsOne()
        {
            var testObj = new Hand(1, 1);
            var result = testObj.GetThePoints(5);
            Assert.That(result, Is.EqualTo(17));
        }

        [Test]
        public void PlayerAcesAreWorthElevenIfNotABust_InOriginalHand()
        {
            var testObj = new Hand(1, 5);
            var result = testObj.GetThePoints(5);
            Assert.That(result, Is.EqualTo(21));
        }

        [Test]
        public void PlayerAcesAreWorthElevenIfNotABust_InOriginalHand_SecondCard()
        {
            var testObj = new Hand(5, 1);
            var result = testObj.GetThePoints(5);
            Assert.That(result, Is.EqualTo(21));
        }
    }
}