using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class HealthBarDisplayer
    {
        internal const String HealthBarTexture = "HealthBar";
        internal const String EmptyHealthBarTexture = "EmptyHealthBar";

        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly SpriteData healthBar;
        private readonly SpriteData emptyHealthBar;
        private readonly Int32 pixelsFromLeft = 32;
        private readonly Int32 pixelsFromBottom = 32;

        internal Int32 MarginOfLeftEdge => pixelsFromLeft;
        internal Int32 HeightOfTopEdge => pixelsFromBottom + healthBar.Height;

        internal HealthBarDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
            this.emptyHealthBar = TexturesHelper.GetSprite(sprites, EmptyHealthBarTexture);
        }

        internal void Draw(PlayerInterfaceInfo interfaceInfo)
        {
            Single healthRemained = interfaceInfo.Health / interfaceInfo.MaxHealth;
            Vector2 position = new Vector2(pixelsFromLeft, interfaceSpriteDisplayer.ScreenHeight - pixelsFromBottom - healthBar.Height);
            interfaceSpriteDisplayer.Draw(healthBar, position, new LeftPartDisplayer(), healthRemained);
            interfaceSpriteDisplayer.Draw(emptyHealthBar, position, new RightPartDisplayer(), 1 - healthRemained);
        }
    }
}
