using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class PlayerWeaponTest
    {
        private TestPlayerInput playerInput = new TestPlayerInput();
        private PlayerWeaponBlueprint blueprint = new PlayerWeaponBlueprint()
        {
            BulletSpeed = 100,
            BulletSpriteName = "FooBar",
            FireRate = 1,
            WeaponRange = 50
        };
        private Int32 shots = 0;
        private List<Single> bulletUpdateTimes = new List<Single>();
        private PlayerWeapon weapon;

        [SetUp]
        public void SetUp()
        {
            shots = 0;
            bulletUpdateTimes = new List<Single>();
            weapon = new PlayerWeapon(blueprint, playerInput, () => new Vector2(0, 0));
            weapon.Shoot += (sender, e) => shots += 1;
            weapon.Shoot += (sender, e) => bulletUpdateTimes.Add(e.FirstUpdateTime);
        }

        [Test]
        public void NoShoot()
        {
            playerInput.StopFire();
            weapon.Check(20);
            playerInput.StopFire();
            Assert.AreEqual(0, shots);
        }

        [Test]
        public void OneShoot()
        {
            playerInput.StartFire();
            weapon.Check(1);
            playerInput.StopFire();
            Assert.AreEqual(1, shots);
        }

        [Test]
        public void BulletAccumulatingError()
        {
            playerInput.StopFire();
            weapon.Check(1);
            weapon.Check(1);
            playerInput.StartFire();
            weapon.Check(0.3F);
            weapon.Check(0.3F);
            weapon.Check(0.4F);
            Assert.AreEqual(1, shots);
        }

        [Test]
        public void FirerateHigherThanFramerate()
        {
            playerInput.StartFire();
            weapon.Check(5);
            Assert.AreEqual(5, shots);
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 4, 3, 2, 1, 0 }));
        }

        [Test]
        public void TwoFramesInRow()
        {
            playerInput.StartFire();
            weapon.Check(2.5F);
            Assert.AreEqual(2, shots);
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 1.5F, 0.5F }));
            bulletUpdateTimes.Clear();
            weapon.Check(2.5F);
            Assert.AreEqual(5, shots);
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 2.0F, 1.0F, 0.0F }));
        }
    }

    internal class TestPlayerInput : IPlayerInput
    {
        private Boolean isFiring = false;

        internal void StartFire()
        {
            isFiring = true;
        }

        internal void StopFire()
        {
            isFiring = false;
        }

        public Vector2 GetFireDirection()
        {
            return new Vector2(1, 0);
        }

        public Vector2 GetMoveDirection()
        {
            throw new System.NotImplementedException();
        }

        public Boolean IsFiring()
        {
            return isFiring;
        }
    }
}
