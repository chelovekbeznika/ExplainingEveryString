﻿using Microsoft.Xna.Framework;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class FirstBossPhaseSpecification
    {
        public EnemyBehaviorSpecification Behavior { get; set; }
        public SpriteSpecification Phase { get; set; }
        public SpriteSpecification On { get; set; }
        public SpriteSpecification Off { get; set; }
        [DefaultValue(null)]
        public Vector2[] TrajectoryParameters { get; set; }
    }
}
