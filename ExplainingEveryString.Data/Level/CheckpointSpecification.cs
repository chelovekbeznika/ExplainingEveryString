using ExplainingEveryString.Data.Specifications;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class CheckpointSpecification
    {
        public const String StartCheckpointName = "Default";

        [DefaultValue(StartCheckpointName)]
        public String Name { get; set; }
        public PositionOnTileMap PlayerPosition { get; set; }
        [DefaultValue(null)]
        public ArsenalSpecification Arsenal { get; set; }
    }
}
