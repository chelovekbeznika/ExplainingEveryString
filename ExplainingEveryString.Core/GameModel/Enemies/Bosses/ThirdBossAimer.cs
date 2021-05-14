using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class ThirdBossAimer : PlayerAimer
    {
        internal Boolean FireSwitch { get; set; }

        internal ThirdBossAimer(Func<Vector2> playerLocator, Func<Vector2> findOutWhereAmI) : base(playerLocator, findOutWhereAmI) { }

        public override Boolean IsFiring()
        {
            return FireSwitch;
        }
    }
}
