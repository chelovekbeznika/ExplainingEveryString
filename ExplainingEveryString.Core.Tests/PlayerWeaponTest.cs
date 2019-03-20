using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

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
        private PlayerWeapon weapon;

        [SetUp]
        public void SetUp()
        {
            shots = 0;
            weapon = new PlayerWeapon(blueprint, playerInput, () => new Vector2(0, 0));
            weapon.Shoot += (sender, e) => shots += 1;
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
