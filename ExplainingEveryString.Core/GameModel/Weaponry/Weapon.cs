using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;

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
        internal event EventHandler<EpicEventArgs> WeaponFired;

        private WeaponReloader reloader;
        private IAimer aimer;
        private Barrel[] barrels;

        private SpecEffectSpecification shootingEffect;

        private Func<Vector2> findOutWhereIAm;
        private Func<Vector2> targetLocator;

        public SpriteState SpriteState { get; private set; }

        public Vector2 Position => findOutWhereIAm();

        public bool IsVisible => SpriteState != null;

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
            reloader = new WeaponReloader(specification, aimer, onShoot);
            SpriteState = specification.Sprite != null ? new SpriteState(specification.Sprite) : null;
            shootingEffect = specification.ShootingEffect;
            this.findOutWhereIAm = findOutWhereIAm;
            this.targetLocator = targetLocator;
            this.WeaponFired += level.EpicEventOccured;
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
            if (IsVisible)
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
        }
    }
}
