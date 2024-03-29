﻿using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class FixedAimer : IAimer
    {
        private Vector2 fireDirection;

        public FixedAimer(Single angle)
        {
            fireDirection = AngleConverter.ToVector(angle);
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

        public void UpdatePosition(Vector2 position)
        {
        }
    }
}
