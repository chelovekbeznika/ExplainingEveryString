using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Text
{
    internal class FontsStorage
    {
        internal CustomFont ScreenResolution { get; private set; }
        internal CustomFont LevelTime { get; private set; }

        internal void LoadContent(ContentManager content)
        {
            ScreenResolution = new ScreenResolutionFont();
            ScreenResolution.Load(content);
            LevelTime = new LevelTimeFont();
            LevelTime.Load(content);
        }
    }
}
