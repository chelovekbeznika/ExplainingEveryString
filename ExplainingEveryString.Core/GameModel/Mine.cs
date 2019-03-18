using ExplainingEveryString.Core.Blueprints;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Mine : GameObject<MineBlueprint>, IUpdatable
    {
        private Boolean alive = true;

        public bool IsAlive()
        {
            return alive;
        }

        public void Update(Single elapsedSeconds)
        {
            throw new NotImplementedException();
        }

        internal void TakeDamage()
        {
            alive = false;
        }
    }
}
