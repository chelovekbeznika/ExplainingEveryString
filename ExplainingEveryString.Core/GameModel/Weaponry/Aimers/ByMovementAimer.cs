using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class ByMovementAimer : IAimer
    {
        private IMovableCollidable shooter;
        private Vector2 lastFireDirection = new Vector2(1, 0);

        internal ByMovementAimer(IMovableCollidable shooter)
        {
            this.shooter = shooter;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            if (shooter.Position != shooter.OldPosition)
                lastFireDirection = (shooter.Position - shooter.OldPosition).NormalizedCopy();
            return lastFireDirection;
        }

        public Boolean IsFiring() => true;

        public void Update(Single elapsedSeconds) { }
    }
}
