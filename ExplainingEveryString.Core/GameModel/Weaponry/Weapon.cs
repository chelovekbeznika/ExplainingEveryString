using Microsoft.Xna.Framework;
using System;
using System.Linq;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Weapon : IDisplayble
    {
        internal event EventHandler<ShootEventArgs> Shoot
        {
            add
            {
                foreach (var barrel in barrels)
                    barrel.Shoot += value;
            }
            remove
            {
                foreach (var barrel in barrels)
                    barrel.Shoot -= value;
            }
        }

        internal String Name { get; private set; }
        internal Reloader Reloader { get; private set; }
        private IAimer aimer;
        private Barrel[] barrels;

        private readonly EpicEvent weaponFired;

        private readonly Func<Vector2> findOutWhereIAm;
        private readonly Func<Vector2> targetLocator;

        public SpriteState SpriteState { get; private set; }
        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();

        public Vector2 Position => findOutWhereIAm();
        public Boolean IsVisible => SpriteState != null;
        internal Boolean IsFiring () => aimer.IsFiring();
        internal Vector2 GetFireDirection() => aimer.GetFireDirection();

        internal Weapon(WeaponSpecification specification, IAimer aimer, 
            Func<Vector2> findOutWhereIAm, Func<Vector2> targetLocator, Level level, Boolean fullAmmoAtStart = false)
        {
            this.aimer = aimer;
            barrels = specification.Barrels
                .Select(bs => new Barrel(aimer, findOutWhereIAm, targetLocator, bs)).ToArray();
            Name = specification.Name;
            Reloader = new Reloader(specification.Reloader, () => aimer.IsFiring(), OnShoot, fullAmmoAtStart);
            SpriteState = specification.Sprite != null ? new SpriteState(specification.Sprite) : null;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetLocator = targetLocator;
            this.weaponFired = new EpicEvent(level, specification.ShootingEffect, false, this, true);
        }

        private void OnShoot(Single seconds)
        {
            foreach (var barrel in barrels)
                barrel.OnShoot(seconds);
        }

        internal void Update(Single elapsedSeconds)
        {
            aimer.Update(elapsedSeconds);
            Reloader.TryReload(elapsedSeconds, out Boolean weaponFired);
            if (weaponFired)
                this.weaponFired.TryHandle();
            if (IsVisible)
            {
                SpriteState.Update(elapsedSeconds);
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
            }
        }
    }
}
