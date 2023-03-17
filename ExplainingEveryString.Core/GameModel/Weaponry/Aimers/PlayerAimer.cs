using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class PlayerAimer : IAimer
    {
        private Func<Vector2> playerLocator;

        internal PlayerAimer(Func<Vector2> playerLocator)
        {
            this.playerLocator = playerLocator;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            Vector2 rawDirection = playerLocator() - currentMuzzlePosition;
            if (rawDirection.Length() > 0)
                return rawDirection / rawDirection.Length();
            else
                return new Vector2(0, 0);
        }

        public virtual Boolean IsFiring() => true;

        public void Update(Single elapsedSeconds)
        {
        }
    }
}
