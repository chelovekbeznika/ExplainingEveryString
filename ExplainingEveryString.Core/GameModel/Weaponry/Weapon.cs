using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Weapon : IDisplayble
    {
        internal event EventHandler<ShootEventArgs> Shoot
        {
            add { barrel.Shoot += value; }
            remove { barrel.Shoot -= value; }
        }
        internal event EventHandler<EpicEventArgs> WeaponFired;

        private WeaponReloader reloader;
        private IAimer aimer;
        private Barrel barrel;

        private SpecEffectSpecification shootingEffect;

        private Func<Vector2> findOutWhereIAm;

        public SpriteState SpriteState { get; private set; }

        public Vector2 Position => findOutWhereIAm();

        public bool IsVisible => SpriteState != null;

        internal Weapon(WeaponSpecification specification, IAimer aimer, Func<Vector2> findOutWhereIAm)
        {
            this.aimer = aimer;
            barrel = new Barrel(aimer, findOutWhereIAm, specification.BulletSpecification, specification.BarrelLength);
            reloader = new WeaponReloader(specification, aimer, barrel.OnShoot);
            SpriteState = specification.Sprite != null ? new SpriteState(specification.Sprite) : null;
            shootingEffect = specification.ShootingEffect;
            this.findOutWhereIAm = findOutWhereIAm;
        }

        internal void Update(Single elapsedSeconds)
        {
            Boolean weaponFired;
            reloader.TryReload(elapsedSeconds, out weaponFired);
            if (weaponFired)
                WeaponFired?.Invoke(this, new EpicEventArgs
                {
                    Position = findOutWhereIAm(),
                    SpecEffectSpecification = shootingEffect
                });
            if (aimer.IsFiring())
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
        }
    }
}
