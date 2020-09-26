using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class ByMovementAimer : IAimer
    {
        IMovableCollidable shooter;

        internal ByMovementAimer(IMovableCollidable shooter)
        {
            this.shooter = shooter;
        }

        public Vector2 GetFireDirection() => (shooter.Position - shooter.OldPosition).NormalizedCopy();

        public Boolean IsFiring() => GetFireDirection() != Vector2.Zero;

        public void Update(Single elapsedSeconds) { }
    }
}
