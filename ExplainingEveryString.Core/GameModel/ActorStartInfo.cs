using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorStartInfo
    {
        internal String BlueprintType { get; set; }
        internal Vector2 Position { get; set; }
        internal Single Angle { get; set; }
        internal List<Vector2> TrajectoryTargets { get; set; }
        internal Single AppearancePhaseDuration { get; set; }
        internal Vector2[] LevelSpawnPoints { get; set; }
    }
}
