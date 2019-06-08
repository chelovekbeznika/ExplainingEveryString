using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorStartInfo
    {
        public String BlueprintType { get; set; }
        public Vector2 Position { get; set; }
        public Single Angle { get; set; }
        public List<Vector2> TrajectoryTargets { get; set; }
    }
}
