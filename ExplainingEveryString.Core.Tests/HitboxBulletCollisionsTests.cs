using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxBulletCollisionsTests
    {
        private const String simpleBulletCollisionCheckCategory = "SimpleBulletCollisionCheck";

        [Test]
        [Category(simpleBulletCollisionCheckCategory)]
        public void Hurt()
        {
            AssertHitboxBulletRelations(new Vector2(0, 0), new Vector2(15, 25), true);
        }

        [Test]
        [Category(simpleBulletCollisionCheckCategory)]
        public void NotHurt()
        {
            AssertHitboxBulletRelations(new Vector2(0, 0), new Vector2(-5, 0), false);
        }

        [Test]
        [Category(simpleBulletCollisionCheckCategory)]
        public void TouchesCorner()
        {
            AssertHitboxBulletRelations(new Vector2(5, 30), new Vector2(10, 30), true);
        }

        [Test]
        public void SimpleCollide()
        {
            AssertHitboxBulletRelations(new Vector2(5, 15), new Vector2(15, 20), true);
            AssertHitboxBulletRelations(new Vector2(20, 20), new Vector2(40, 30), true);
        }

        [Test]
        public void GoesThrough()
        {
            AssertHitboxBulletRelations(new Vector2(0, 10), new Vector2(40, 30), true);
            AssertHitboxBulletRelations(new Vector2(10, 40), new Vector2(30, 0), true);
        }

        [Test]
        public void BulletGoesThroughVertically()
        {
            AssertHitboxBulletRelations(new Vector2(0, 0), new Vector2(0, 40), false);
            AssertHitboxBulletRelations(new Vector2(10, 0), new Vector2(10, 40), true);
            AssertHitboxBulletRelations(new Vector2(20, 0), new Vector2(20, 40), true);
            AssertHitboxBulletRelations(new Vector2(20, -20), new Vector2(20, 0), false);
            AssertHitboxBulletRelations(new Vector2(30, 0), new Vector2(30, 40), true);
            AssertHitboxBulletRelations(new Vector2(40, 0), new Vector2(40, 40), false);
        }

        [Test]
        public void BulletGoesThroughHorizontally()
        {
            AssertHitboxBulletRelations(new Vector2(0, 0), new Vector2(40, 0), false);
            AssertHitboxBulletRelations(new Vector2(0, 10), new Vector2(40, 10), true);
            AssertHitboxBulletRelations(new Vector2(0, 20), new Vector2(40, 20), true);
            AssertHitboxBulletRelations(new Vector2(-20, 20), new Vector2(0, 20), false);
            AssertHitboxBulletRelations(new Vector2(0, 30), new Vector2(40, 30), true);
            AssertHitboxBulletRelations(new Vector2(0, 40), new Vector2(40, 40), false);
        }

        private void AssertHitboxBulletRelations(Vector2 oldPosition, Vector2 newPosition, Boolean hurt)
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            Hitbox hitbox = new Hitbox { Left = 10, Right = 30, Top = 30, Bottom = 10 };
            Assert.That(collisionsChecker.Collides(hitbox, oldPosition, newPosition), Is.EqualTo(hurt));
        }
    }
}
