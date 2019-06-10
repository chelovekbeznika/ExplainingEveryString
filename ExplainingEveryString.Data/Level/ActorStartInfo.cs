using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class ActorStartInfo
    {
        public String BlueprintType { get; set; }
        public PositionOnTileMap TilePosition { get; set; }
        [DefaultValue(0)]
        public Single Angle { get; set; }
        [DefaultValue(null)]
        public List<Vector2> TrajectoryTargets { get; set; }
        [DefaultValue(0)]
        public Single AppearancePhaseDuration { get; set; }
    }
}
