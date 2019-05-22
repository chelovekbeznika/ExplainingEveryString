using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class FixedAimer : IAimer
    {
        private Vector2 fireDirection;

        public FixedAimer(Single angle)
        {
            fireDirection = AngleConverter.ToVector(angle);
        }

        public Vector2 GetFireDirection()
        {
            return fireDirection;
        }

        public bool IsFiring()
        {
            return true;
        }
    }
}
