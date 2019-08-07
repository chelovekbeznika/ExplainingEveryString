using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpawnerSpecification
    {
        public Int32 MaxSpawned { get; set; }
        public ReloaderSpecification Reloader { get; set; }
        public String BlueprintType { get; set; }
        [JsonIgnore]
        public virtual SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.LevelSpawnPoints;

    }

    public class RandomSpawnerSpecification : SpawnerSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.RandomInCircle;
        public Single SpawnRadius { get; set; }
    }

    public class RelativeSpawnerSpecificaton : SpawnerSpecification
    {
        public override SpawnPositionSelectionType PositionSelectionType => SpawnPositionSelectionType.RelativeToSpawner;
        public Vector2[] SpawnPositions { get; set; }
    }

    public enum SpawnPositionSelectionType { LevelSpawnPoints, RandomInCircle, RelativeToSpawner };
}
