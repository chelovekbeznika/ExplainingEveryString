using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Weapon : IDisplayble
    {
        internal event EventHandler<ShootEventArgs> Shoot
        {
            add
            {
                foreach (Barrel barrel in barrels)
                    barrel.Shoot += value;
            }
            remove
            {
                foreach (Barrel barrel in barrels)
                    barrel.Shoot -= value;
            }
        }

        private Reloader reloader;
        private IAimer aimer;
        private Barrel[] barrels;

        private EpicEvent WeaponFired;

        private Func<Vector2> findOutWhereIAm;
        private Func<Vector2> targetLocator;

        public SpriteState SpriteState { get; private set; }

        public Vector2 Position => findOutWhereIAm();
        public Boolean IsVisible => SpriteState != null;
        internal Boolean IsFiring () => aimer.IsFiring();
        internal Vector2 GetFireDirection() => aimer.GetFireDirection();

        internal Weapon(WeaponSpecification specification, IAimer aimer, 
            Func<Vector2> findOutWhereIAm, Func<Vector2> targetLocator, Level level)
        {
            this.aimer = aimer;
            barrels = specification.Barrels
                .Select(bs => new Barrel(aimer, findOutWhereIAm, targetLocator, bs)).ToArray();
            Action<Single> onShoot = new Action<Single>((seconds) => { });
            foreach (Action<Single> barellShoot in barrels.Select(b => new Action<Single>(b.OnShoot)))
            {
                onShoot += barellShoot;
            }
            reloader = new Reloader(specification.Reloader, () => aimer.IsFiring(), onShoot);
            SpriteState = specification.Sprite != null ? new SpriteState(specification.Sprite) : null;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetLocator = targetLocator;
            this.WeaponFired = new EpicEvent(level, specification.ShootingEffect, false, this, true);
        }

        internal void Update(Single elapsedSeconds)
        {
            Boolean weaponFired;
            reloader.TryReload(elapsedSeconds, out weaponFired);
            if (weaponFired)
                WeaponFired.TryHandle();
            if (IsVisible)
            {
                SpriteState.Update(elapsedSeconds);
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
            }
        }
    }
}
