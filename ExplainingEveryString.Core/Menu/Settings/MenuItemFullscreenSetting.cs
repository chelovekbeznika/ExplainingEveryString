using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class MenuItemFullscreenSetting : MenuItem, IMenuItemDisplayble
    {
        private Dictionary<bool, Texture2D> sprites;

        internal override BorderType BorderType => BorderType.Setting;

        internal override IMenuItemDisplayble Displayble => this;

        private bool Fullscreen
        {
            get => SettingsAccess.GetCurrentSettings().Fullscreen;
            set => SettingsAccess.GetCurrentSettings().Fullscreen = value;
        }

        internal MenuItemFullscreenSetting(Texture2D window, Texture2D fullscreen)
        {
            sprites = new Dictionary<bool, Texture2D>
            {
                { false, window },
                { true, fullscreen }
            };
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprites[Fullscreen], position, Color.White);
        }

        public Point GetSize() => new Point(sprites[Fullscreen].Width, sprites[Fullscreen].Height);

        internal override void RequestCommandExecution()
        {
            Fullscreen = !Fullscreen;
        }

        internal override void Decrement()
        {
            Fullscreen = !Fullscreen;
        }

        internal override void Increment()
        {
            Fullscreen = !Fullscreen;
        }
    }
}
