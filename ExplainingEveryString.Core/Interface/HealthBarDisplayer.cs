using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class HealthBarDisplayer
    {
        private Texture2D healthBar;
        private Texture2D emptyHealthBar;
        private readonly Int32 pixelsFromLeft = 32;
        private readonly Int32 pixelsFromBottom = 32;

        internal HealthBarDisplayer(Texture2D healthBar, Texture2D emptyHealthBar)
        {
            this.healthBar = healthBar;
            this.emptyHealthBar = emptyHealthBar;
        }

        internal void Draw(Single health, Single maxHealth, SpriteBatch spriteBatch, Color colorMask)
        {
            var (healthBarPosition, emptyHealthBarPosition) = 
                CalculatePlaceForHealthBar(spriteBatch.GraphicsDevice.Viewport, health, maxHealth);
            var (healthBarPart, emptyHealthBarPart) = 
                CalculateHealthBarDrawPart(health, maxHealth);

            spriteBatch.Draw(healthBar, healthBarPosition, healthBarPart, colorMask);
            spriteBatch.Draw(emptyHealthBar, emptyHealthBarPosition, emptyHealthBarPart, colorMask);
        }

        private ValueTuple<Vector2, Vector2> CalculatePlaceForHealthBar
            (Viewport viewport, Single health, Single maxHealth)
        {
            Int32 x = pixelsFromLeft;
            Single healthRemained = health / maxHealth;
            Int32 y = viewport.Height - pixelsFromBottom - healthBar.Height;
            return (new Vector2(x, y), new Vector2((Int32)(x + healthBar.Width * healthRemained), y));
        }

        private ValueTuple<Rectangle, Rectangle> CalculateHealthBarDrawPart(Single health, Single maxHealth)
        {
            Single healthRemained = health / maxHealth;
            Rectangle visiblePart = new Rectangle
            {
                X = 0,
                Y = 0,
                Height = healthBar.Height,
                Width = (Int32)(healthBar.Width * healthRemained)
            };
            Rectangle emptyPart = new Rectangle
            {
                X = visiblePart.Width,
                Y = 0,
                Height = visiblePart.Height,
                Width = healthBar.Width - visiblePart.Width
            };
            return (visiblePart, emptyPart);
        }
    }
}
