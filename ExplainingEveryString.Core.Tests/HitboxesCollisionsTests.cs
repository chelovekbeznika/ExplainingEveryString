using ExplainingEveryString.Core.GameModel;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxesCollisionsTests
    {
        private CollisionsChecker collisionsChecker;

        [SetUp]
        public void CreateCollisionsChecker()
        {
            collisionsChecker = new CollisionsChecker();
        }

        [Test]
        public void NotCollides()
        {
            Hitbox first = new Hitbox { Bottom = 40, Top = 60, Left = 40, Right = 60 };
            Hitbox second = new Hitbox { Bottom = 0, Top = 10, Left = 0, Right = 10 };
            AssertHitboxesRelations(false, first, second);
        }

        [Test]
        public void IntersectOnlyOnOneAxis()
        {
            Hitbox first = new Hitbox { Bottom = 0, Top = 20, Left = 0, Right = 20 };
            Hitbox second = new Hitbox { Bottom = 100, Top = 120, Left = 10, Right = 30 };
            AssertHitboxesRelations(false, first, second);
        }

        [Test]
        public void Collides()
        {
            Hitbox first = new Hitbox { Bottom = 10, Top = 30, Left = 10, Right = 30 };
            Hitbox second = new Hitbox { Bottom = 20, Top = 40, Left = 20, Right = 40 };
            AssertHitboxesRelations(true, first, second);
        }

        [Test]
        public void OneHitboxInsideOfOther()
        {
            Hitbox first = new Hitbox { Bottom = 0, Top = 40, Left = 0, Right = 40 };
            Hitbox second = new Hitbox { Bottom = 10, Top = 30, Left = 10, Right = 30 };
            AssertHitboxesRelations(true, first, second);
        }

        [Test]
        public void TouchingHitboxes()
        {
            Hitbox first = new Hitbox { Bottom = 0, Top = 20, Left = 0, Right = 20 };
            Hitbox second = new Hitbox { Bottom = 20, Top = 40, Left = 10, Right = 30 };
            AssertHitboxesRelations(true, first, second);
            second = new Hitbox { Bottom = 10, Top = 30, Left = 20, Right = 40 };
            AssertHitboxesRelations(true, first, second);
        }

        [Test]
        public void TouchingByCornerHitboxes()
        {
            Hitbox first = new Hitbox { Bottom = 0, Top = 20, Left = 0, Right = 20 };
            Hitbox second = new Hitbox { Bottom = 20, Top = 40, Left = 20, Right = 40 };
            AssertHitboxesRelations(true, first, second);
        }

        [Test]
        public void DifferentAxis()
        {
            Hitbox first = new Hitbox { Bottom = -20, Top = 20, Left = -10, Right = 30 };
            Hitbox second = new Hitbox { Bottom = -40, Top = -10, Left = -20, Right = 10 };
            AssertHitboxesRelations(true, first, second);
        }

        [Test]
        public void HitboxLiesAtRight()
        {
            Hitbox first = new Hitbox { Bottom = 0, Top = 20, Left = 0, Right = 20 };
            Hitbox second = new Hitbox { Bottom = 0, Top = 20, Left = 40, Right = 60 };
            AssertHitboxesRelations(false, first, second);
        }

        private void AssertHitboxesRelations(Boolean collides, Hitbox first, Hitbox second)
        {
            AssertHitboxesCollides(collides, first, second);
            first = FlipAroundXAxis(first);
            second = FlipAroundXAxis(second);
            AssertHitboxesCollides(collides, first, second);
            first = FlipAroundYAxis(first);
            second = FlipAroundYAxis(second);
            AssertHitboxesCollides(collides, first, second);
        }

        private Hitbox FlipAroundXAxis(Hitbox hitbox)
        {
            return new Hitbox { Bottom = hitbox.Bottom, Top = hitbox.Top, Left = -hitbox.Right, Right = -hitbox.Left };
        }

        private Hitbox FlipAroundYAxis(Hitbox hitbox)
        {
            return new Hitbox { Bottom = -hitbox.Top, Top = -hitbox.Bottom, Left = hitbox.Left, Right = hitbox.Right };
        }

        private void AssertHitboxesCollides(Boolean collides, Hitbox first, Hitbox second)
        {
            Assert.That(collisionsChecker.Collides(first, second) == collides);
            Assert.That(collisionsChecker.Collides(second, first) == collides);
        }
    }
}
