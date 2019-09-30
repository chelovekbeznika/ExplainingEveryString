using System;
using System.ComponentModel;

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
