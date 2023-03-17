using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class FixedPostTeleportCorrectionAimer : IAimer
    {
        private Vector2 fireDirection;
        private Func<Vector2> playerLocator;
        private Func<Vector2> findOutWhereAmI;

        public FixedPostTeleportCorrectionAimer(Single angle, Func<Vector2> playerLocator, 
            Func<Vector2> findOutWhereAmI, IEnemyBehavior enemyBehavior)
        {
            this.fireDirection = AngleConverter.ToVector(angle);
            this.playerLocator = playerLocator;
            this.findOutWhereAmI = findOutWhereAmI;
            enemyBehavior.MoveGoalReached += RecalibrateAim;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            return fireDirection;
        }

        public Boolean IsFiring()
        {
            return true;
        }

        public void Update(Single elapsedSeconds)
        {
        }

        private void RecalibrateAim(Object sender, EventArgs e)
        {
            Vector2 rawDirection = playerLocator() - findOutWhereAmI();
            if (rawDirection.Length() > 0)
                fireDirection = rawDirection / rawDirection.Length();
            else
                fireDirection = new Vector2(0, 0);
        }
    }
}
