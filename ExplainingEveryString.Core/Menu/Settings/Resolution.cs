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

        internal static List<Resolution> GetSupportedResolutions(GraphicsAdapter adapter)
        {
            Resolution displayModeToResolution(DisplayMode dp) => new Resolution { Width = dp.Width, Height = dp.Height };
            return adapter.SupportedDisplayModes.Select(displayModeToResolution).Distinct().ToList();
        }

        public override String ToString() => $"{Width}x{Height}";
    }
}
