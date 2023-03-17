using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBossAimer : PlayerAimer
    {
        internal Boolean FireSwitch { get; set; }

        internal ThirdBossAimer(Func<Vector2> playerLocator) : base(playerLocator) { }

        public override Boolean IsFiring()
        {
            return FireSwitch;
        }
    }
}
