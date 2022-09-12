using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FifthBoss : Enemy<FifthBossBlueprint>
    {
        private Weapon leftWeapon;
        private Weapon rightWeapon;
        private FifthBossEye leftEye;
        private FifthBossEye rightEye;
        private FifthBossWeaponMovement leftWeaponMovement;
        private FifthBossWeaponMovement rightWeaponMovement;

        protected override void Construct(FifthBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.leftWeaponMovement = new FifthBossWeaponMovement(blueprint.LeftWeaponMovementCycle);
            this.rightWeaponMovement = new FifthBossWeaponMovement(blueprint.RightWeaponMovementCycle);
            Vector2 playerLocator() => level.Player.Position;
            Vector2 leftWeaponLocator() => Position + leftWeaponMovement.CurrentOffset;
            Vector2 rightWeaponLocator() => Position + rightWeaponMovement.CurrentOffset;
            var leftAimer = new PlayerAimer(playerLocator, leftWeaponLocator);
            var rightAimer = new PlayerAimer(playerLocator, rightWeaponLocator);
            this.leftWeapon = new Weapon(blueprint.LeftWeapon, leftAimer, leftWeaponLocator, () => level.Player, level);
            leftWeapon.Shoot += level.EnemyShoot;
            this.rightWeapon = new Weapon(blueprint.RightWeapon, rightAimer, rightWeaponLocator, () => level.Player, level);
            rightWeapon.Shoot += level.EnemyShoot;
            this.leftEye = new FifthBossEye(this, blueprint.LeftEyeOffset, blueprint.EyeSprite, leftWeaponMovement);
            this.rightEye = new FifthBossEye(this, blueprint.RightEyeOffset, blueprint.EyeSprite, rightWeaponMovement);
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (!IsInAppearancePhase)
            {
                leftWeaponMovement.Update(elapsedSeconds);
                rightWeaponMovement.Update(elapsedSeconds);
                leftWeapon.Update(elapsedSeconds);
                rightWeapon.Update(elapsedSeconds);
                leftEye.Update(elapsedSeconds);
                rightEye.Update(elapsedSeconds);
            }
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            return base.GetParts().Concat(new[] { leftEye, rightEye });
        }
    }
}
