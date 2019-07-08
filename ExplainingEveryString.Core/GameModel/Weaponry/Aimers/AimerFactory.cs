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
        internal static IAimer Get(WeaponSpecification weapon, ActorStartInfo actorStartInfo,
            Func<Vector2> currentPositionLocator, Func<Vector2> playerLocator)
        {
            switch (weapon.AimType)
            {
                case AimType.FixedFireDirection:
                    return new FixedAimer(actorStartInfo.Angle);
                case AimType.AimAtPlayer:
                    return new PlayerAimer(playerLocator, currentPositionLocator);
                default:
                    throw new ArgumentException("Wrong aimtype in blueprint");
            }
        }
    }
}
