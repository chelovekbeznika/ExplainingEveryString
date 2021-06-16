using ExplainingEveryString.Core.Displaying;
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
        private const Int32 BaseHeight = 224;
        private const Int32 BaseWidth = 256;

        internal static List<Resolution> AllowedResolutions(this GraphicsAdapter adapter, Boolean fullscreen)
        {
            if (fullscreen)
            {
                return adapter.SupportedDisplayModes
                    .Select(dp => new Resolution { Width = dp.Width, Height = dp.Height })
                    .Distinct().ToList();
            }
            else
            {
                var maxHeight = adapter.SupportedDisplayModes.Max(dp => dp.Height);
                var resolutions = maxHeight / BaseHeight;
                return Enumerable.Range(1, resolutions)
                    .Select(n => new Resolution { Width = Constants.BaseWidth * n, Height = Constants.BaseHeiht * n })
                    .ToList();
            }
        }
    }
}
