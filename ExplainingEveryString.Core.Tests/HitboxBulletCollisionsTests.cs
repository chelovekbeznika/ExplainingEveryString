using System;
using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxBulletCollisionsTests
    {
        [Test]
        public void Hurt()
        {
            AssertHitboxBulletRelations(new Vector2(15, 25), true);
        }

        [Test]
        public void NotHurt()
        {
            AssertHitboxBulletRelations(new Vector2(-5, 0), false);
        }

        [Test]
        public void TouchesCorner()
        {
            AssertHitboxBulletRelations(new Vector2(10, 30), true);
        }

        private void AssertHitboxBulletRelations(Vector2 point, Boolean hurt)
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            Hitbox hitbox = new Hitbox { Left = 10, Right = 30, Top = 30, Bottom = 10 };
            Assert.That(collisionsChecker.Collides(hitbox, point) == hurt);
        }
    }
}
