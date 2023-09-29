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
        private readonly IAimer aimer;
        private readonly Barrel[] barrels;

        private readonly EpicEvent weaponFired;
        private readonly EpicEvent reloadStarted;
        private readonly EpicEvent reloadFinished;

        private readonly Func<Vector2> findOutWhereIAm;

        public SpriteState SpriteState { get; private set; }
        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();

        public Vector2 Position => findOutWhereIAm();
        public Boolean IsVisible => SpriteState != null;
        internal Boolean IsFiring() => aimer.IsFiring();
        internal Boolean IsHoming => barrels.Any(barrel => barrel.IsHoming);
        internal Vector2 GetFireDirection() => aimer.GetFireDirection(findOutWhereIAm());

        internal Weapon(WeaponSpecification specification, IAimer aimer, 
            Func<Vector2> findOutWhereIAm, Func<IActor> targetSelector, Level level, Boolean fullAmmoAtStart = false)
        {
            this.aimer = aimer;
            this.findOutWhereIAm = () => findOutWhereIAm() + specification.Offset;
            barrels = specification.Barrels
                .Select(bs => new Barrel(level, aimer, this.findOutWhereIAm, targetSelector, bs)).ToArray();
            Name = specification.Name;
            Reloader = new Reloader(specification.Reloader, () => aimer.IsFiring(), OnShoot, fullAmmoAtStart);
            Reloader.ReloadStarted += (sender, e) => reloadStarted.TryHandle();
            Reloader.ReloadFinished += (sender, e) => reloadFinished.TryHandle();
            SpriteState = specification.Sprite != null ? new SpriteState(specification.Sprite) : null;
            weaponFired = new EpicEvent(level, specification.ShootingEffect, false, this, true);
            reloadStarted = new EpicEvent(level, specification.Reloader.ReloadStartedEffect, false, this, true);
            reloadFinished = new EpicEvent(level, specification.Reloader.ReloadFinishedEffect, false, this, true);
        }

        private void OnShoot(Single seconds)
        {
            foreach (var barrel in barrels)
                barrel.OnShoot(seconds);
        }

        internal void Update(Single elapsedSeconds)
        {
            aimer.Update(elapsedSeconds);
            Reloader.Update(elapsedSeconds, out Boolean weaponFired);
            if (weaponFired)
                this.weaponFired.TryHandle();
            if (IsVisible)
            {
                SpriteState.Update(elapsedSeconds);
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection(findOutWhereIAm()));
            }
        }
    }
}
