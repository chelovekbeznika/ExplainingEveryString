﻿using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class EnemyWave
    {
        [DefaultValue(null)]
        public DoorStartInfo[] Doors { get; set; }
        [DefaultValue(null)]
        public CheckpointSpecification Checkpoint { get; set; }
        public Rectangle StartRegion { get; set; }
        [DefaultValue(Int32.MaxValue)]
        public Int32 MaxEnemiesAtOnce { get; set; }
        public ActorStartInfo[] Enemies { get; set; }
        [DefaultValue(null)]
        public ActorStartInfo[] Bosses { get; set; }
        [DefaultValue(null)]
        public String RoomName { get; set; }
    }
}
