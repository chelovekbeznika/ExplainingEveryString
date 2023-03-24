using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.RandomVariables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class LastBossPhase : Enemy<LastBossBlueprint>
    {
        private Weapon[] weapons;
        private Single tillNextChange;
        private GaussRandomVariable betweenChanges;
        private Int32 simultaneuosly;
        private List<Int32> activeWeapons;
        private EpicEvent weaponsChanged;

        protected override void Construct(LastBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            weapons = blueprint.Weapons.Select(ws =>
            {
                var angle = AngleConverter.ToRadians(ws.Angle);
                var aimer = AimersFactory.Get(ws.Weapon.AimType, angle, this, () => level.Player.Position, null);
                var weapon = new Weapon(ws.Weapon, aimer, () => Position, () => level.Player, level);
                weapon.Shoot += level.EnemyShoot;
                return weapon;
            }).ToArray();
            weaponsChanged = new EpicEvent(level, blueprint.WeaponsChangeEffect, false, this, false, true);
            betweenChanges = blueprint.WeaponsChangeInterval;
            tillNextChange = 0;
            simultaneuosly = blueprint.SimultaneouslyInUse;
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);

            // Simple way to fix cat alignment during phase #4
            SpriteState.Angle = 0;

            if (!IsInAppearancePhase)
            {
                tillNextChange -= elapsedSeconds;
                if (tillNextChange <= 0)
                {
                    tillNextChange = RandomUtility.NextGauss(betweenChanges);
                    ChangeWeapons();
                }
                foreach (var (weapon, index) in weapons.Select((weapon, index) => (weapon, index)))
                {
                    if (activeWeapons.Contains(index))
                        weapon.Update(elapsedSeconds);
                }
            }
        }

        private void ChangeWeapons()
        {
            activeWeapons = RandomUtility.IntsFromRange(simultaneuosly, weapons.Length);
            weaponsChanged?.TryHandle();
        }
    }
}
