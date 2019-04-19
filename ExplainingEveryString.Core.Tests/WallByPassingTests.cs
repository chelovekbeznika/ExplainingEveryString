using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class WallByPassingTests
    {
        private Hitbox wall = new Hitbox { Left = 30, Right = 50, Bottom = 30, Top = 50 };
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

        [Test]
        public void MovingTowardToWall()
        {
            Hitbox oldPosition = new Hitbox { Left = 0, Right = 20, Bottom = 0, Top = 20 };
            Hitbox newPosition = new Hitbox { Left = 5, Right = 25, Bottom = 5, Top = 25 };
            AssertWallIsNotAffectingMovement(oldPosition, newPosition);

            oldPosition = new Hitbox { Left = -20, Right = 0, Bottom = 0, Top = 20 };
            newPosition = new Hitbox { Left = -10, Right = 10, Bottom = 20, Top = 40 };
            AssertWallIsNotAffectingMovement(oldPosition, newPosition);
        }

        [Test]
        public void MovingFromWall()
        {
            Hitbox oldPosition = new Hitbox { Left = 0, Right = 20, Bottom = 0, Top = 20 };
            Hitbox newPosition = new Hitbox { Left = -10, Right = 10, Bottom = -5, Top = 15 };
            AssertWallIsNotAffectingMovement(oldPosition, newPosition);
        }

        [Test]
        public void TouchingWall()
        {
            Hitbox oldPosition = new Hitbox { Left = 0, Right = 20, Bottom = 0, Top = 20 };
            Hitbox newPosition = new Hitbox { Left = 10, Right = 30, Bottom = 20, Top = 40 };
            AssertWallIsNotAffectingMovement(oldPosition, newPosition);
        }

        [Test]
        public void TryToRideThroughWallFromBottom()
        {
            Hitbox oldPosition = new Hitbox { Left = 10, Right = 40, Bottom = -40, Top = -10 };
            Hitbox newPosition = new Hitbox { Left = 50, Right = 80, Bottom = 70, Top = 100 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 65, Y = 15 }));
            Assert.That(corner, Is.EqualTo(false));
        }

        [Test]
        public void TryToRideThroughWallFromRight()
        { 
            Hitbox oldPosition = new Hitbox { Left = 70, Right = 100, Bottom = 25, Top = 55 };
            Hitbox newPosition = new Hitbox { Left = 40, Right = 70, Bottom = 25, Top = 55 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 65, Y = 40 }));
            Assert.That(corner, Is.EqualTo(false));
        }

        [Test]
        public void TryToRideThroughWallFromLeftWhileTouchingIt()
        {
            Hitbox oldPosition = new Hitbox { Left = 0, Right = 30, Bottom = 20, Top = 50 };
            Hitbox newPosition = new Hitbox { Left = 90, Right = 120, Bottom = 50, Top = 80 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 15, Y = 65 }));
            Assert.That(corner, Is.EqualTo(false));
        }

        [Test]
        public void TryToRideThroughWallFromRightWhileTouchingIt()
        {
            Hitbox oldPosition = new Hitbox { Left = 50, Right = 80, Bottom = 20, Top = 50 };
            Hitbox newPosition = new Hitbox { Left = -20, Right = 10, Bottom = 50, Top = 80 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 65, Y = 65 }));
            Assert.That(corner, Is.EqualTo(false));
        }

        [Test]
        public void TryToRideThroughWallFromTop()
        {
            Hitbox oldPosition = new Hitbox { Left = 10, Right = 40, Bottom = 60, Top = 90 };
            Hitbox newPosition = new Hitbox { Left = 20, Right = 50, Bottom = 30, Top = 60 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 35, Y = 65 }));
            Assert.That(corner, Is.EqualTo(false));
        }

        [Test]
        public void TryToRideThroughWallFromBottomWhileTouchingIt()
        {
            Hitbox oldPosition = new Hitbox { Left = 0, Right = 30, Bottom = 0, Top = 30 };
            Hitbox newPosition = new Hitbox { Left = 40, Right = 70, Bottom = 10, Top = 40 };
            Vector2? actualPosition = null;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, true, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 55, Y = 15 }));
            Assert.That(corner, Is.EqualTo(true));
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actualPosition, false, out corner);
            Assert.That(actualPosition.Value, Is.EqualTo(new Vector2 { X = 15, Y = 25 }));
            Assert.That(corner, Is.EqualTo(true));
        }

        private void AssertWallIsNotAffectingMovement(Hitbox oldPosition, Hitbox newPosition)
        {
            Vector2? actual;
            Boolean corner;
            collisionsChecker.TryToBypassWall(oldPosition, newPosition, wall, out actual, true, out corner);
            Assert.That(actual, Is.EqualTo(null));
            Assert.That(corner, Is.EqualTo(false));
        }
    }
}
