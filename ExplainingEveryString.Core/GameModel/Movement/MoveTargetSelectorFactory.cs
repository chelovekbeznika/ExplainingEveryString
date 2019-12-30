using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using ExplainingEveryString.Core.GameModel.Movement.TargetSelectors;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    internal static class MoveTargetSelectorFactory
    {
        internal static IMoveTargetSelector Get(MoveTargetSelectType type, Vector2[] parameters, 
            Func<Vector2> playerLocator, ICollidable actor)
        {
            Vector2 actorLocator() => actor.Position;
            switch (type)
            {
                case MoveTargetSelectType.NoTarget:
                    return new NotATargetSelector(actorLocator);
                case MoveTargetSelectType.TargetsList:
                    return new LoopingTargetSelector(parameters, actorLocator());
                case MoveTargetSelectType.RandomFromListTargets:
                    return new RandomFromListTargetsSelector(parameters, actorLocator());
                case MoveTargetSelectType.MoveTowardPlayer:
                    return new PlayerHunter(playerLocator);
                case MoveTargetSelectType.RandomTargets:
                    return new RandomFlight(actor, parameters[0], parameters[1]);
                default:
                    throw new ArgumentException("Unknow type of MoveTargetSelectType");
            }
        }
    }
}
