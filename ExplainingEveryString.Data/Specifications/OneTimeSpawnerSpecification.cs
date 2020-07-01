using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class OneTimeSpawnerSpecification
    {
        public String BlueprintType { get; set; }
        public Int32 MaxSpawned { get; set; }
        public Single Interval { get; set; }
    }
}
