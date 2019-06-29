using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class ReloaderSpecification
    {
        public Single FireRate { get; set; }
        [DefaultValue(1)]
        public Int32 Ammo { get; set; }
        [DefaultValue(0.0)]
        public Single ReloadTime { get; set; }
    }
}
