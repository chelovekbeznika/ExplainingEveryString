using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class SoundSpecification
    {
        public String Name { get; set; }
        public Single Volume { get; set; }
        [DefaultValue(1)]
        public Single FadingCoeff { get; set; }
    }
}
