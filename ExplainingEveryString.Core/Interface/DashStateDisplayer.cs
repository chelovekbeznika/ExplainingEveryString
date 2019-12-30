using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class DashStateDisplayer
    {
        internal const String AvailableTexture = "FullDash";
        internal const String NonAvailableTexture = "FullDashNotReady";
        internal const String CooldownTexture = "EmptyDash";
        internal const String ActiveTexture = "ActiveDash";

        private readonly SpriteData available;
        private readonly SpriteData nonAvailable;
        private readonly SpriteData cooldown;
        private readonly SpriteData active;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly HealthBarDisplayer healthBarDisplayer;
        private Int32 PixelsFromLeft => healthBarDisplayer.MarginOfLeftEdge - 16;
        private Int32 PixelsFromBottom => healthBarDisplayer.HeightOfTopEdge;

        internal DashStateDisplayer(HealthBarDisplayer healthBarDisplayer, InterfaceSpriteDisplayer animationController,
            Dictionary<String, SpriteData> sprites)
        {
            this.healthBarDisplayer = healthBarDisplayer;
            this.interfaceSpriteDisplayer = animationController;
            this.available = TexturesHelper.GetSprite(sprites, AvailableTexture);
            this.nonAvailable = TexturesHelper.GetSprite(sprites, NonAvailableTexture);
            this.cooldown = TexturesHelper.GetSprite(sprites, CooldownTexture);
            this.active = TexturesHelper.GetSprite(sprites, ActiveTexture);
        }

        internal void Draw(PlayerInterfaceInfo interfaceInfo)
        {
            var barPosition = new Vector2
            {
                X = PixelsFromLeft,
                Y = interfaceSpriteDisplayer.ScreenHeight - PixelsFromBottom - available.Height
            };
            var cooldownRemained = interfaceInfo.TillDashRecharge / interfaceInfo.DashCooldown;
            switch (interfaceInfo.DashState)
            {
                case DashState.Active:
                    interfaceSpriteDisplayer.Draw(active, barPosition);
                    break;
                case DashState.Nonavailable:
                    interfaceSpriteDisplayer.Draw(cooldown, barPosition, new LeftPartDisplayer(), cooldownRemained);
                    interfaceSpriteDisplayer.Draw(nonAvailable, barPosition, new RightPartDisplayer(), 1 - cooldownRemained);
                    break;
                case DashState.Available:
                    interfaceSpriteDisplayer.Draw(cooldown, barPosition, new LeftPartDisplayer(), cooldownRemained);
                    interfaceSpriteDisplayer.Draw(available, barPosition, new RightPartDisplayer(), 1 - cooldownRemained);
                    break;
            }
        }
    }
}
