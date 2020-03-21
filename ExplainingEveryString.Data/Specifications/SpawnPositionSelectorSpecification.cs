﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public abstract class SpawnPositionSelectorSpecification
    {
        [JsonIgnore]
        public abstract SpawnPositionSelectionType PositionSelectionType { get; }
    }

    public class LevelSpawnPositionSelectorSpecification : SpawnPositionSelectorSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.LevelSpawnPoints;
    }

    public class RandomSpawnPositionSelectorSpecification : SpawnPositionSelectorSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.RandomInCircle;
        public Single SpawnRadius { get; set; }
    }

    public class RelativeSpawnPositionSelectorSpecificaton : SpawnPositionSelectorSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.RelativeToSpawner;
        public Vector2[] SpawnPositions { get; set; }
    }

    public class CustomSpawnPositionSelectorSpecification : SpawnPositionSelectorSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.Custom;
        [DefaultValue(0)]
        public Int32 BetweenRepeats { get; set; }
    }

    public enum SpawnPositionSelectionType { LevelSpawnPoints, RandomInCircle, RelativeToSpawner, Custom };
}
