using System;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal struct Resolution
    {
        internal Int32 Width { get; set; }
        internal Int32 Height { get; set; }

        public override String ToString() => $"{Width}x{Height}";
    }
}
