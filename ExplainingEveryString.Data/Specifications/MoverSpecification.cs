using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class MoverSpecification : IModificationSpecification
    { 
        public MoveType Type { get; set; }
        public Dictionary<String, Single> Parameters { get; set; }
        [DefaultValue("Mover")]
        public String ModificationType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MoveType
    {
        StayingStill,
        Linear,
        LinearWithStops,
        Acceleration,
        Teleportation,
        Axis
    }
}
