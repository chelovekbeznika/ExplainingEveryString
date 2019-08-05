using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class PostMortemSurpriseSpecification
    {
        public BarrelSpecification[] Barrels { get; set; }
        public AimType AimType { get; set; }
    }
}
