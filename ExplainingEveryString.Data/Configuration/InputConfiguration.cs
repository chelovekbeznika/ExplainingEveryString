using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Configuration
{
    public class InputConfiguration
    {
        public ControlDevice ControlDevice { get; set; }
        public Single TimeToFocusOnKeyboard { get; set; }
        public Single TimeToFocusOnGamepad { get; set; }
    }
}
