using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemMusicVolumeSetting : MenuItem
    {
        private const Int32 pixelsBetween = 4;

        private Texture2D empty;
        private Texture2D full;
        private Int32 maxBars;
        private Int32 barsSelected;

        internal MenuItemMusicVolumeSetting(Texture2D empty, Texture2D full, Int32 maxBars)
        {
            this.empty = empty;
            this.full = full;
            this.maxBars = maxBars;
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var x = position.X;
            foreach (var index in Enumerable.Range(0, maxBars))
            {
                var sprite = index < barsSelected ? full : empty;
                var y = position.Y + System.Math.Max(empty.Height, full.Height) - sprite.Height;
                spriteBatch.Draw(sprite, new Vector2(x, y), Color.White);
                x += sprite.Width + pixelsBetween;
            }
        }

        internal override Point GetSize() => new Point
        (
            x: barsSelected * full.Width + (maxBars - barsSelected) * empty.Width + pixelsBetween * (maxBars - 1),
            y: System.Math.Max(empty.Height, full.Height)
        );

        internal override void RequestCommandExecution()
        {
            Increment();
        }

        internal override void Increment()
        {
            if (barsSelected < maxBars)
                barsSelected += 1;
        }

        internal override void Decrement()
        {
            if (barsSelected > 0)
                barsSelected -= 1;
        }
    }
}
