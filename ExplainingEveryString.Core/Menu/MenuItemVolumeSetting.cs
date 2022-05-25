using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemVolumeSetting : MenuItem
    {
        private const Int32 pixelsBetween = 4;

        private Texture2D empty;
        private Texture2D full;
        private Int32 maxBars;

        private Func<Int32> getBarsSelected;
        private Action<Int32> setBarsSelected;

        internal override BorderType BorderType => BorderType.Setting;

        internal MenuItemVolumeSetting(Texture2D empty, Texture2D full, Int32 maxBars, Func<Int32> getItem, Action<Int32> setItem)
        {
            this.empty = empty;
            this.full = full;
            this.maxBars = maxBars;
            this.getBarsSelected = getItem;
            this.setBarsSelected = setItem;
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            var x = position.X;
            foreach (var index in Enumerable.Range(0, maxBars))
            {
                var sprite = index < getBarsSelected() ? full : empty;
                var y = position.Y + System.Math.Max(empty.Height, full.Height) - sprite.Height;
                spriteBatch.Draw(sprite, new Vector2(x, y), Color.White);
                x += sprite.Width + pixelsBetween;
            }
        }

        internal override Point GetSize() => new Point
        (
            x: getBarsSelected() * full.Width + (maxBars - getBarsSelected()) * empty.Width + pixelsBetween * (maxBars - 1),
            y: System.Math.Max(empty.Height, full.Height)
        );

        internal override void RequestCommandExecution()
        {
            Increment();
        }

        internal override void Increment()
        {
            if (getBarsSelected() < maxBars)
                setBarsSelected(getBarsSelected() + 1);
        }

        internal override void Decrement()
        {
            if (getBarsSelected() > 0)
                setBarsSelected(getBarsSelected() - 1);
        }
    }
}
