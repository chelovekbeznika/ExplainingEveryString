﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Trajectories
{
    internal abstract class BulletTrajectory
    {
        private Vector2 startPosition;
        private Vector2 fireDirection;

        internal BulletTrajectory(Vector2 startPosition, Vector2 fireDirection, Dictionary<String, Single> parameters)
        {
            this.startPosition = startPosition;
            this.fireDirection = fireDirection;
            AssignParameters(parameters);
        }

        internal Vector2 GetBulletPosition(Single time)
        {
            Vector2 rawPosition = GetTrajectoryOffset(time);
            Single sinus = fireDirection.Y;
            Single cosinus = fireDirection.X;
            Vector2 rotatedPosition = new Vector2
            {
                X = rawPosition.X * cosinus - rawPosition.Y * sinus,
                Y = rawPosition.X * sinus + rawPosition.Y * cosinus
            };
            return startPosition + rotatedPosition;
        }

        protected abstract void AssignParameters(Dictionary<String, Single> parameters);
        protected abstract Vector2 GetTrajectoryOffset(Single time);
    }
}
