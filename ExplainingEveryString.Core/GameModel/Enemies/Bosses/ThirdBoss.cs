using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBoss : Enemy<ThirdBossBlueprint>
    {
        private Weapon[] smallWeapons;
        private ThirdBossAimersController aimersController;

        protected override void Construct(ThirdBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);

            var aimers = new List<ThirdBossAimer>();
            var weapons = new List<Weapon>();
            foreach (var weaponOffset in blueprint.SmallWeaponOffsets)
            {
                Vector2 weaponPositionLocator() => Position + weaponOffset;
                var aimer = new ThirdBossAimer(() => level.Player.Position, weaponPositionLocator);
                aimers.Add(aimer);
                var weapon = new Weapon(blueprint.SmallWeapon, aimer, weaponPositionLocator, () => level.Player, level);
                weapon.Shoot += level.EnemyShoot;
                weapons.Add(weapon);
            }
            smallWeapons = weapons.ToArray();
            aimersController = new ThirdBossAimersController(blueprint.Aimers, aimers.ToArray());
        }

        public override IEnumerable<IDisplayble> GetParts()
        {
            if (!IsInAppearancePhase)
                return smallWeapons.Concat(base.GetParts());
            else
                return base.GetParts();
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (!IsInAppearancePhase)
            {
                foreach (var weapon in smallWeapons)
                    weapon.Update(elapsedSeconds);
                aimersController.Update(elapsedSeconds);
            }
        }
    }
}
