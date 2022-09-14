using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBossLimb : IUpdateable, IDisplayble
    {
        private Weapon weapon;
        private FifthBossLimbDisplayble displayble;
        private FifthBossWeaponMovement weaponMovement;

        public FifthBossLimb(FifthBossLimbSpecification specification, FifthBoss boss, Level level)
        {
            this.weaponMovement = new FifthBossWeaponMovement(specification.WeaponMovementCycle);
            Vector2 weaponLocator() => boss.Position + weaponMovement.CurrentOffset;
            var aimer = specification.Angle is null 
                ? new PlayerAimer(() => level.Player.Position, weaponLocator) as IAimer
                : new FixedAimer(AngleConverter.ToRadians(specification.Angle.Value)) as IAimer;
            this.weapon = new Weapon(specification.Weapon, aimer, weaponLocator, () => level.Player, level);
            weapon.Shoot += level.EnemyShoot;
            this.displayble = new FifthBossLimbDisplayble(boss, specification.Offset, specification.Sprite, weaponMovement);
        }

        public SpriteState SpriteState => displayble.SpriteState;

        public Vector2 Position => displayble.Position;

        public bool IsVisible => displayble.IsVisible;

        public IEnumerable<IDisplayble> GetParts() => displayble.GetParts();

        public void Update(Single elapsedSeconds)
        {
            weaponMovement.Update(elapsedSeconds);
            weapon.Update(elapsedSeconds);
            displayble.Update(elapsedSeconds);
        }
    }
}
