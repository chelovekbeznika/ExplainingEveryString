using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Vector2 GetFireDirection() => AngleConverter.ToVector(angle);

        public bool IsFiring() => true;

        public void Update(float elapsedSeconds)
        {
            angle += elapsedSeconds * angleSpeed;
        }
    }
}
