using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class PlayerWeaponTest
    {
        private TestAimer playerInput = new TestAimer();
        private WeaponSpecification specification = new WeaponSpecification()
        {
            BulletSpecification = new BulletSpecification
            {
                Speed = 100,
                Sprite = new SpriteSpecification { Name = "FooBar" },
                Range = 50,
                Damage = 1
            },
            FireRate = 1,
        };
        private Int32 shots = 0;
        private List<Single> bulletUpdateTimes = new List<Single>();
        private Weapon weapon;

        [SetUp]
        public void SetUp()
        {
            shots = 0;
            bulletUpdateTimes = new List<Single>();
            weapon = new Weapon(specification, playerInput, () => new Vector2(0, 0));
            weapon.Shoot += (sender, e) => shots += 1;
            weapon.Shoot += (sender, e) => bulletUpdateTimes.Add(e.FirstUpdateTime);
        }

        [Test]
        public void NoShoot()
        {
            playerInput.StopFire();
            weapon.Update(20);
            playerInput.StopFire();
            Assert.That(shots, Is.EqualTo(0));
        }

        [Test]
        public void OneShoot()
        {
            playerInput.StartFire();
            weapon.Update(1);
            playerInput.StopFire();
            Assert.That(shots, Is.EqualTo(1));
        }

        [Test]
        public void BulletAccumulatingError()
        {
            playerInput.StopFire();
            weapon.Update(1);
            weapon.Update(1);
            playerInput.StartFire();
            weapon.Update(0.3F);
            weapon.Update(0.3F);
            weapon.Update(0.4F);
            Assert.That(shots, Is.EqualTo(1));
        }

        [Test]
        public void FirerateHigherThanFramerate()
        {
            playerInput.StartFire();
            weapon.Update(5);
            Assert.That(shots, Is.EqualTo(5));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 4, 3, 2, 1, 0 }));
        }

        [Test]
        public void TwoFramesInRow()
        {
            playerInput.StartFire();
            weapon.Update(2.5F);
            Assert.That(shots, Is.EqualTo(2));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 1.5F, 0.5F }));
            bulletUpdateTimes.Clear();
            weapon.Update(2.5F);
            Assert.That(shots, Is.EqualTo(5));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 2.0F, 1.0F, 0.0F }));
        }

        [Test]
        public void RareShooting()
        {
            weapon.Update(2);
            playerInput.StartFire();
            foreach (Int32 index in Enumerable.Range(0, 5))
                weapon.Update(0.2F);
            Assert.That(shots, Is.EqualTo(2));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 0, 0 }));
            bulletUpdateTimes.Clear();
            foreach (Int32 index in Enumerable.Range(0, 5))
                weapon.Update(0.2F);
            Assert.That(shots, Is.EqualTo(3));
            Assert.That(bulletUpdateTimes, Is.EquivalentTo(new List<Single> { 0 }));
        }
    }

    internal class TestAimer : IAimer
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

        public Boolean IsFiring()
        {
            return isFiring;
        }
    }
}
