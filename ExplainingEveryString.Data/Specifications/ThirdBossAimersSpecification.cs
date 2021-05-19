using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Specifications
{
    public class ThirdBossAimersSpecification
    {
        public List<Int32> HealthThresholds { get; set; }
        public List<Int32> SimultaneouslyFiring { get; set; }
        public Single ChangeInterval { get; set; }
    }
}
