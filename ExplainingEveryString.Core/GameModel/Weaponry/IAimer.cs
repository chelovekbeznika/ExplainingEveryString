using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal interface IAimer
    {
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }

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
