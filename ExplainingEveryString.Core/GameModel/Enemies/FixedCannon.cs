using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class FixedCannon : Enemy<FixedCannonBlueprint>, ICrashable, ITouchableByBullets
    {
        private IAimer aimer;
        private Weapon weapon;
        private Single startAngle;

        protected override void PlaceOnLevel(GameObjectStartPosition position)
        {
            base.PlaceOnLevel(position);
            startAngle = position.Angle;
        }

        protected override void Construct(FixedCannonBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            aimer = CreateAimer(blueprint.Weapon);
            weapon = new Weapon(blueprint.Weapon, aimer, () => this.Position, level);
            weapon.Shoot += level.EnemyShoot;
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            weapon.Update(elapsedSeconds);
            if (aimer.IsFiring() && !weapon.IsVisible)
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
        }

        private IAimer CreateAimer(WeaponSpecification weapon)
        {
            switch (weapon.AimType)
            {
                case AimType.FixedFireDirection:
                    return new FixedAimer(AngleConverter.ToRadians(startAngle));
                case AimType.AimAtPlayer:
                    return new PlayerAimer(PlayerLocator, () => this.Position);
                default:
                    throw new ArgumentException("Wrong aimtype in blueprint");
            }
        }
    }
}
