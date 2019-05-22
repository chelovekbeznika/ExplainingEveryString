using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class BarrelSpecification
    {
        public Single Length { get; set; }
        [DefaultValue(0.0)]
        public Single AngleCorrection { get; set; }
        [DefaultValue(0.0)]
        public Single Accuracy { get; set; }
        public BulletSpecification Bullet { get; set; }
    }
}
