using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class DashStateDisplayer
    {
        private readonly Texture2D available;
        private readonly Texture2D nonAvailable;
        private readonly Texture2D cooldown;
        private readonly String activeSpriteName;
        private readonly AnimationController animationController;
        private readonly HealthBarDisplayer healthBarDisplayer;
        private Int32 PixelsFromLeft => healthBarDisplayer.MarginOfLeftEdge - 16;
        private Int32 PixelsFromBottom => healthBarDisplayer.HeightOfTopEdge;

        internal DashStateDisplayer(HealthBarDisplayer healthBarDisplayer, AnimationController animationController, 
            Texture2D available, Texture2D nonAvailable, Texture2D cooldown, String activeSpriteName)
        {
            this.healthBarDisplayer = healthBarDisplayer;
            this.animationController = animationController;
            this.available = available;
            this.nonAvailable = nonAvailable;
            this.cooldown = cooldown;
            this.activeSpriteName = activeSpriteName;
        }

        internal void Draw(PlayerInterfaceInfo interfaceInfo, SpriteBatch spriteBatch, Color colorMask)
        {
            Viewport viewport = spriteBatch.GraphicsDevice.Viewport;
            Vector2 barPosition = new Vector2 { X = PixelsFromLeft, Y = viewport.Height - PixelsFromBottom - available.Height };
            switch (interfaceInfo.DashState)
            {
                case DashState.Active:
                    animationController.Draw(spriteBatch, colorMask, activeSpriteName, barPosition);
                    break;
                case DashState.Nonavailable:
                    DrawPartiallyCharged(interfaceInfo, spriteBatch, colorMask, barPosition, nonAvailable);
                    break;
                case DashState.Available:
                    DrawPartiallyCharged(interfaceInfo, spriteBatch, colorMask, barPosition, available);
                    break;
            }
        }

        private void DrawPartiallyCharged(PlayerInterfaceInfo interfaceInfo, SpriteBatch spriteBatch, Color colorMask, 
            Vector2 barPosition, Texture2D filledSprite)
        {
            Int32 width = available.Width;
            Single cooldownCoeff = interfaceInfo.TillDashRecharge / interfaceInfo.DashCooldown;
            Int32 cooldownPartPixels = (Int32)(width * cooldownCoeff);
            Int32 rechargedPartPixels = width - cooldownPartPixels;
            DrawCooldownPart(spriteBatch, colorMask, cooldownPartPixels, barPosition);
            DrawRechargedPart(spriteBatch, colorMask, filledSprite, rechargedPartPixels, cooldownPartPixels, barPosition);
        }

        private void DrawCooldownPart(SpriteBatch spriteBatch, Color colorMask, 
            Int32 cooldownPartPixels, Vector2 barPosition)
        {
            if (cooldownPartPixels > 0)
            {
                Vector2 position = barPosition;
                Rectangle part = new Rectangle
                {
                    X = 0,
                    Y = 0,
                    Width = cooldownPartPixels,
                    Height = cooldown.Height
                };
                spriteBatch.Draw(cooldown, position, part, colorMask);
            }
        }

        private void DrawRechargedPart(SpriteBatch spriteBatch, Color colorMask, Texture2D filledSprite, 
            Int32 rechargedPartPixels, Int32 cooldownPartPixels, Vector2 barPosition)
        {
            Vector2 position = new Vector2 { X = barPosition.X + cooldownPartPixels, Y = barPosition.Y };
            Rectangle part = new Rectangle
            {
                X = cooldownPartPixels,
                Y = 0,
                Width = rechargedPartPixels,
                Height = available.Height
            };
            spriteBatch.Draw(filledSprite, position, part, colorMask);
        }
    }
}
