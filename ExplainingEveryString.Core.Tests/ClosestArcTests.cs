using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class ClosestArcTests
    {
        [Test]
        public void SimpleCounterClockTest()
        {
            var arc = AngleConverter.ClosestArc(MathHelper.Pi / 6, MathHelper.PiOver2);
            Assert.That(arc, Is.EqualTo(MathHelper.Pi / 3).Within(Constants.Epsilon));
            arc = AngleConverter.ClosestArc(-MathHelper.PiOver2, MathHelper.PiOver4);
            Assert.That(arc, Is.EqualTo(MathHelper.Pi / 4 * 3).Within(Constants.Epsilon));
        }

        [Test]
        public void SimpleClockwiseTest()
        {
            var arc = AngleConverter.ClosestArc(MathHelper.PiOver2, MathHelper.Pi / 6);
            Assert.That(arc, Is.EqualTo(-MathHelper.Pi / 3).Within(Constants.Epsilon));
            arc = AngleConverter.ClosestArc(MathHelper.PiOver2, -MathHelper.PiOver4);
            Assert.That(arc, Is.EqualTo(-MathHelper.Pi / 4 * 3).Within(Constants.Epsilon));
        }

        [Test]
        public void DifferentSidesCounterClockTest()
        {
            var arc = AngleConverter.ClosestArc(MathHelper.Pi / 6 * 5, -MathHelper.Pi / 6 * 5);
            Assert.That(arc, Is.EqualTo(MathHelper.Pi / 3).Within(Constants.Epsilon));
            arc = AngleConverter.ClosestArc(MathHelper.Pi, -MathHelper.PiOver2);
            Assert.That(arc, Is.EqualTo(MathHelper.PiOver2).Within(Constants.Epsilon));
        }

        [Test]
        public void DifferentSidesClockwise()
        {
            var arc = AngleConverter.ClosestArc(-MathHelper.Pi / 6 * 5, MathHelper.Pi / 6 * 5);
            Assert.That(arc, Is.EqualTo(-MathHelper.Pi / 3).Within(Constants.Epsilon));
            arc = AngleConverter.ClosestArc(-MathHelper.PiOver2, MathHelper.Pi);
            Assert.That(arc, Is.EqualTo(-MathHelper.PiOver2).Within(Constants.Epsilon));
        }

        [Test]
        public void SamePoint()
        {
            var arc = AngleConverter.ClosestArc(-MathHelper.Pi, MathHelper.Pi);
            Assert.That(arc, Is.EqualTo(0).Within(Constants.Epsilon));
        }
    }
}
