using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class PlayerHunter : IMoveTargetSelector
    {
        private Func<Vector2> playerLocator;

        internal PlayerHunter(Func<Vector2> playerLocator)
        {
            this.playerLocator = playerLocator;
        }

        public Vector2 GetTarget()
        {
            return playerLocator();
        }

        public void SwitchToNextTarget()
        {

        }
    }
}
