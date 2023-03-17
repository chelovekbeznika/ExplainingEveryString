using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossFixedDirectionAimer : IAimer
    {
        private FourthBossPart bossPart;

        internal FourthBossFixedDirectionAimer(FourthBossPart bossPart)
        {
            this.bossPart = bossPart;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition) => AngleConverter.ToVector(bossPart.BossBrain.Angle);

        public bool IsFiring() => true;

        public void Update(Single elapsedSeconds)
        {
        }
    }
}
