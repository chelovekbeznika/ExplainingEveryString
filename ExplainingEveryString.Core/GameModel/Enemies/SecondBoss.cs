using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class SecondBoss : Enemy<SecondBossBlueprint>
    {
        private Player player;
        private Single deathZoneEllipseFocusRadius;
        private Single deathZoneEllipseFocusDistanceSum;
        private Single deathZoneDamage;
        private Single inDeathZone;

        protected override void Construct(SecondBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.player = level.Player;
            var a = blueprint.DeathEllipseX;
            var b = blueprint.DeathEllipseY;
            this.deathZoneEllipseFocusRadius = (Single)System.Math.Sqrt(a * a - b * b);
            this.deathZoneEllipseFocusDistanceSum = 2 * a;
            this.deathZoneDamage = blueprint.DeathZoneDamage;
        }

        public override void Update(Single elapsedSeconds)
        {
            DeathZoneControl(elapsedSeconds);
            base.Update(elapsedSeconds);
        }

        private void DeathZoneControl(Single elapsedSeconds)
        {
            if (PlayerInDeathZone())
            {
                inDeathZone += elapsedSeconds;
                var damage = deathZoneDamage * (inDeathZone * inDeathZone - (inDeathZone - elapsedSeconds) * (inDeathZone - elapsedSeconds));
                player.TakeDamage(damage);
            }
            else
                inDeathZone = 0;
        }

        private Boolean PlayerInDeathZone()
        {
            var focus1 = Position + new Vector2(-deathZoneEllipseFocusRadius, 0);
            var focus2 = Position + new Vector2(deathZoneEllipseFocusRadius, 0);
            return (player.Position - focus1).Length() + (player.Position - focus2).Length() > deathZoneEllipseFocusDistanceSum;
        }
    }
}
