using System;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpawnerSpecification
    {
        public Int32 MaxSpawned { get; set; }
        public ReloaderSpecification Reloader { get; set; }
        public String BlueprintType { get; set; }
        public SpawnPositionSelectorSpecification PositionSelector { get; set; }
    }
}
