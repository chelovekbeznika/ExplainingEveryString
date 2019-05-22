using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class PlayerAimer : IAimer
    {
        private Func<Vector2> playerLocator;
        private Func<Vector2> findOutWhereAmI;

        internal PlayerAimer(Func<Vector2> playerLocator, Func<Vector2> findOutWhereAmI)
        {
            this.playerLocator = playerLocator;
            this.findOutWhereAmI = findOutWhereAmI;
        }

        public Vector2 GetFireDirection()
        {
            Vector2 rawDirection = playerLocator() - findOutWhereAmI();
            if (rawDirection.Length() > 0)
                return rawDirection / rawDirection.Length();
            else
                return new Vector2(0, 0);
        }

        public bool IsFiring()
        {
            return true;
        }
    }
}
