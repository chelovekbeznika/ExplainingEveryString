using System;
using NUnit.Framework;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class LimitedAmmoReloadTests : ReloadTests
    {
        public override void SetUp()
        {
            specification.FireRate = 1;
            specification.Ammo = 5;
            specification.ReloadTime = 10;
            base.SetUp();
        }
        
        [Test]
        public void BeforeShot()
        {
            InnerAssert(5, 0, 0);
        }

        [Test]
        public void FirstShot()
        {
            InnerAssert(10, 1, 1);
        }

        [Test]
        public void SimpleReloadTest()
        {
            InnerAssert(20, 5, 1);         
        }

        [Test]
        public void OneClipNotUsed()
        {
            InnerAssert(13, 4, 1);
        }

        [Test]
        public void OneTimeReloadedAndNextClipStarted()
        {
            InnerAssert(24, 6, 2);
        }

        [Test]
        public void TwoClipsUsed()
        {
            InnerAssert(29, 10, 2);
        }

        private void InnerAssert(Single time, Int32 shots, Int32 reloadsFinished)
        {
            aimer.StartFire();
            reloader.Update(time, out _);
            aimer.StopFire();
            Assert.That(this.shots, Is.EqualTo(shots));
            Assert.That(this.reloadsFinished, Is.EqualTo(reloadsFinished));
        }
    }
}
