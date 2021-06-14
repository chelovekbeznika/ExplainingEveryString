using ExplainingEveryString.Core.Menu.Settings;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.Extensions
{
    internal static class GraphicsAdapterExtensions
    {
        internal static List<Resolution> AllowedResolutions(this GraphicsAdapter adapter)
        {
            return adapter.SupportedDisplayModes
                .Select(dp => new Resolution { Width = dp.Width, Height = dp.Height })
                .Distinct().ToList();
        }
    }
}
