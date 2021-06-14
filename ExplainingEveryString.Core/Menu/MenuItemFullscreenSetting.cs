using ExplainingEveryString.Core.Menu.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemFullscreenSetting : MenuItem
    {
        private Dictionary<Boolean, Texture2D> sprites;

        internal override BorderType BorderType => BorderType.Setting;

        private Boolean Fullscreen
        {
            get => SettingsAccess.GetCurrentSettings().Fullscreen;
            set => SettingsAccess.GetCurrentSettings().Fullscreen = value;
        }

        internal MenuItemFullscreenSetting(Texture2D window, Texture2D fullscreen)
        {
            this.sprites = new Dictionary<Boolean, Texture2D>
            {
                { false, window },
                { true, fullscreen }
            };
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprites[Fullscreen], position, Color.White);
        }

        internal override Point GetSize() => new Point(sprites[Fullscreen].Width, sprites[Fullscreen].Height);

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
