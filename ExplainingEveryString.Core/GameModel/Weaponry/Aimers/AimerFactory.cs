﻿using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal static class AimersFactory
    {
        internal static IAimer Get(AimType aimType, Single angle,
            IMovableCollidable shooter, Func<Vector2> playerLocator)
        {
            switch (aimType)
            {
                case AimType.FixedFireDirection:
                    return new FixedAimer(angle);
                case AimType.AimAtPlayer:
                    return new PlayerAimer(playerLocator, () => shooter.Position);
                case AimType.ByMovement:
                    return new ByMovementAimer(shooter);
                case AimType.SpinningAim:
                    return new SpinningAimer(angle);
                default:
                    throw new ArgumentException("Wrong aimtype in blueprint");
            }
        }
    }
}
