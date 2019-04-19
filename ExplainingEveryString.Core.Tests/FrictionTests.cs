using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class FrictionTests
    {
        [OneTimeSetUp]
        public void FixtureSetup()
        {
            FrictionCorrector.FrictionCoefficient = 0.5F;
        }

        [Test]
        public void OneSecond()
        {
            AssertSlowing(100, 50, 1);
        }

        [Test]
        public void StopsWhenVerySlow()
        {
            AssertSlowing(100, 0, 10);
            AssertSlowing(5, 0, 0.01F);
        }

        [Test]
        public void HalfSecondStep()
        {
            Single halfSecondSpeed = 100 * (Single)(1 / System.Math.Sqrt(2));
            AssertSlowing(100, halfSecondSpeed, 0.5F);
            AssertSlowing(halfSecondSpeed, 50, 0.5F);
        }

        public void SimpleSlowingDown()
        {
            AssertSlowing(20, 5, 2);
            AssertSlowing(100, 12.5F, 3);
        }

        private void AssertSlowing(Single before, Single after, Single elapsedTime)
        {
            Vector2 start = new Vector2(before, 0);
            Assert.That(FrictionCorrector.Correct(start, elapsedTime), Is.EqualTo(new Vector2(after, 0)));
        }
    }
}
