using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{

    internal class TestAimer : IAimer
    {
        private Boolean isFiring = false;

        internal void StartFire()
        {
            isFiring = true;
        }

        internal void StopFire()
        {
            isFiring = false;
        }

        public Vector2 GetFireDirection()
        {
            return new Vector2(1, 0);
        }

        public Boolean IsFiring()
        {
            return isFiring;
        }
    }
}
