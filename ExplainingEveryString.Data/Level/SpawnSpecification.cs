using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class SpawnSpecification
    {
        public Vector2 SpawnPoint { get; set; }
        [DefaultValue(null)]
        public Vector2[] TrajectoryParameters { get; set; }
        [DefaultValue(0)]
        public Single Angle { get; set; }
    }
}
