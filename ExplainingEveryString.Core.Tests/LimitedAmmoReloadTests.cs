using System;
using ExplainingEveryString.Data.Blueprints;
using NUnit.Framework;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class LimitedAmmoReloadTests : ReloadTests
    {
        public override void SetUp()
        {
            specification.Ammo = 5;
            specification.ReloadTime = 10;
            base.SetUp();
        }
        
        [Test]
        public void BeforeShot()
        {
            reloader.TryReload(50, out Boolean weaponFired);
            InnerAssert(5, 0);
        }

        [Test]
        public void FirstShot()
        {
            InnerAssert(10, 1);
        }

        [Test]
        public void SimpleReloadTest()
        {
            InnerAssert(20, 5);         
        }

        [Test]
        public void OneClipNotUsed()
        {
            InnerAssert(14, 4);
        }

        [Test]
        public void OneTimeReloadedAndNextClipStarted()
        {
            InnerAssert(21, 7);
        }

        [Test]
        public void TwoClipsUsed()
        {
            InnerAssert(29, 10);
            InnerAssert(30, 11);
        }

        private void InnerAssert(Single time, Int32 shots)
        {
            aimer.StartFire();
            reloader.TryReload(time, out Boolean weaponFired);
            aimer.StopFire();
            Assert.That(shots, Is.EqualTo(shots));
        }
    }
}
