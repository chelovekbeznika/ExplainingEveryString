using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal static class AimersFactory
    {
        internal static IAimer Get(AimType aimType, Single angle,
            Func<Vector2> currentPositionLocator, Func<Vector2> playerLocator)
        {
            switch (aimType)
            {
                case AimType.FixedFireDirection:
                    return new FixedAimer(angle);
                case AimType.AimAtPlayer:
                    return new PlayerAimer(playerLocator, currentPositionLocator);
                default:
                    throw new ArgumentException("Wrong aimtype in blueprint");
            }
        }
    }
}
