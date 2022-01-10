using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorStartInfo
    {
        internal String BlueprintType { get; set; }
        internal Vector2 Position { get; set; }
        internal BehaviorParameters BehaviorParameters { get; set; }
        internal Single AppearancePhaseDuration { get; set; }
        internal Object[] AdditionalParameters { get; set; }
    }
}
