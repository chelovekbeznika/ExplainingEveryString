using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class MenuItemVolumeSetting : MenuItem, IMenuItemDisplayble
    {
        private const int pixelsBetween = 4;

        private readonly Texture2D empty;
        private readonly Texture2D full;
        private readonly int maxBars;

        private readonly Func<int> getBarsSelected;
        private readonly Action<int> setBarsSelected;

        internal override BorderType BorderType => BorderType.Setting;

        internal override IMenuItemDisplayble Displayble => this;

        internal MenuItemVolumeSetting(Texture2D empty, Texture2D full, int maxBars, Func<int> getItem, Action<int> setItem)
        {
            this.empty = empty;
            this.full = full;
            this.maxBars = maxBars;
            getBarsSelected = getItem;
            setBarsSelected = setItem;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
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

        public Point GetSize() => new Point
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
