﻿using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class BehaviorParameters
    {
        internal Single Angle { get; set; }
        internal Vector2[] TrajectoryParameters { get; set; }
        internal Vector2[] LevelSpawnPoints { get; set; }
    }
}
