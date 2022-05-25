using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Specifications
{
    public class ThirdBossAimersSpecification
    {
        public List<Int32> HealthThresholds { get; set; }
        public List<Int32> SimultaneouslyFiring { get; set; }
        public Single ChangeInterval { get; set; }
    }
}
