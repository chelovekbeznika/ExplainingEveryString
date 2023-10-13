using System;

namespace ExplainingEveryString.Data.Configuration
{
    public class InputConfiguration
    {
        public ControlDevice PreferredControlDevice { get; set; }
        public Single TimeToFocusOnKeyboard { get; set; }
        public Single TimeToFocusOnGamepad { get; set; }
        public Single BetweenPlayerAndCursor { get; set; }
    }
}
