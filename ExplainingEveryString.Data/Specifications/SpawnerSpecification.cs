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
        public SpawnPositionSelectorSpecification PositionSelector { get; set; }
    }
}
