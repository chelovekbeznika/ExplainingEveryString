using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class RotatingAimer : IAimer
    {
        private Single angleSpeed;
        private Single angle = 0;

        internal RotatingAimer(Single angleSpeed)
        {
            this.angleSpeed = angleSpeed;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition) => AngleConverter.ToVector(angle);

        public bool IsFiring() => true;

        public void Update(float elapsedSeconds)
        {
            angle += elapsedSeconds * angleSpeed;
        }
    }
}
