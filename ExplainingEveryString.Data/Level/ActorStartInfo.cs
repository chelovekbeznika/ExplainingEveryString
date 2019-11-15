using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class ActorStartInfo
    {
        public String BlueprintType { get; set; }
        public PositionOnTileMap TilePosition { get; set; }
        [DefaultValue(0)]
        public Single Angle { get; set; }
        [DefaultValue(null)]
        public Vector2[] TrajectoryParameters { get; set; }
        [DefaultValue(0)]
        public Single AppearancePhaseDuration { get; set; }
    }
}
