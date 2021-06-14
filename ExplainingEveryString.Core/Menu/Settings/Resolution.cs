using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal struct Resolution
    {
        internal Int32 Width { get; set; }
        internal Int32 Height { get; set; }

        public override String ToString() => $"{Width}x{Height}";
    }
}
