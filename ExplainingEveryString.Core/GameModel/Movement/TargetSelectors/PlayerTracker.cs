using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class PlayerTracker : IMoveTargetSelector
    {
        private Func<Vector2> playerLocator;
        private Vector2 lastPlayerTrace;

        internal PlayerTracker(Func<Vector2> playerLocator)
        {
            this.playerLocator = playerLocator;
            this.lastPlayerTrace = playerLocator();
        }

        public Vector2 GetTarget()
        {
            return lastPlayerTrace;
        }

        public void SwitchToNextTarget()
        {
            lastPlayerTrace = playerLocator();
        }
    }
}
