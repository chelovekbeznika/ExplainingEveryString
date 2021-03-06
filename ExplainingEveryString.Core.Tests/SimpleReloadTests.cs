﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class SimpleReloadTests : ReloadTests
    {
        [Test]
        public void NoShoot()
        {
            aimer.StopFire();
            var weaponFired = false;
            reloader.Update(20, out weaponFired);
            aimer.StopFire();
            Assert.That(shots, Is.EqualTo(0));
            Assert.That(weaponFired, Is.EqualTo(false));
        }

        [Test]
        public void OneShoot()
        {
            aimer.StartFire();
            var weaponFired = false;
            reloader.Update(1, out weaponFired);
            aimer.StopFire();
            Assert.That(shots, Is.EqualTo(1));
            Assert.That(weaponFired, Is.EqualTo(true));
        }

        [Test]
        public void BulletAccumulatingError()
        {
            aimer.StopFire();
            var weaponFired = false;
            reloader.Update(1, out weaponFired);
            reloader.Update(1, out weaponFired);
            aimer.StartFire();
            reloader.Update(0.3F, out weaponFired);
            reloader.Update(0.3F, out weaponFired);
            reloader.Update(0.4F, out weaponFired);
            Assert.That(shots, Is.EqualTo(1));
        }

        [Test]
        public void FirerateHigherThanFramerate()
        {
            aimer.StartFire();
            var weaponFired = false;
            reloader.Update(5, out weaponFired);
            Assert.That(shots, Is.EqualTo(5));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 4, 3, 2, 1, 0 }));
        }

        [Test]
        public void TwoFramesInRow()
        {
            var weaponFired = false;
            aimer.StartFire();
            reloader.Update(2.5F, out weaponFired);
            Assert.That(shots, Is.EqualTo(2));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 1.5F, 0.5F }));
            bulletUpdateTimes.Clear();
            reloader.Update(2.5F, out weaponFired);
            Assert.That(shots, Is.EqualTo(5));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 2.0F, 1.0F, 0.0F }));
        }

        [Test]
        public void RareShooting()
        {
            var weaponFired = false;
            reloader.Update(2, out weaponFired);
            aimer.StartFire();
            foreach (var index in Enumerable.Range(0, 5))
                reloader.Update(0.2F, out weaponFired);
            Assert.That(shots, Is.EqualTo(2));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 0, 0 }));
            bulletUpdateTimes.Clear();
            foreach (var index in Enumerable.Range(0, 5))
                reloader.Update(0.2F, out weaponFired);
            Assert.That(shots, Is.EqualTo(3));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 0 }));
        }
    }
}
