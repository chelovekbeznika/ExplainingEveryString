using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ExplainingEveryString.Core.GameModel.Movement.TargetSelectors;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal static class MoveTargetSelectorFactory
    {
        internal static IMoveTargetSelector Get(MoveTargetSelectType type, List<Vector2> targets, 
            Func<Vector2> playerLocator, Func<Vector2> currentPositionLocator)
        {
            switch (type)
            {
                case MoveTargetSelectType.NoTarget:
                    return new NotATargetSelector(currentPositionLocator);
                case MoveTargetSelectType.TargetsList:
                    return new LoopingTargetSelector(targets, currentPositionLocator());
                case MoveTargetSelectType.MoveTowardPlayer:
                    return new PlayerHunter(playerLocator);
                case MoveTargetSelectType.RandomTargets:
                    return new RandomFlight(currentPositionLocator(), targets[0]);
                default:
                    throw new ArgumentException("Unknow type of MoveTargetSelectType");
            }
        }
    }
}
