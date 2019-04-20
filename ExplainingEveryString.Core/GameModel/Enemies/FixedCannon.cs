using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
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

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            weapon.Update(elapsedSeconds);
            if (aimer.IsFiring() && !weapon.IsVisible)
                SpriteState.Angle = AngleConverter.ToRadians(aimer.GetFireDirection());
        }

        protected override void Construct(FixedCannonBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            aimer = new FixedAimer(MathHelper.PiOver4);
            weapon = new Weapon(blueprint.Weapon, aimer, () => this.Position, level);
            weapon.Shoot += level.EnemyShoot;
        }
    }
}
