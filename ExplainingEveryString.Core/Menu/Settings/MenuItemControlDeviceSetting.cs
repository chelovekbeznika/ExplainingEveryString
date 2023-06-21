using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal class MenuItemControlDeviceSetting : MenuItem, IMenuItemDisplayble
    {
        private Dictionary<ControlDevice, Texture2D> sprites;
        private ControlDevice SelectedDevice
        {
            get => SettingsAccess.GetCurrentSettings().PreferrableControlDevice;
            set => SettingsAccess.GetCurrentSettings().PreferrableControlDevice = value;
        }

        internal override BorderType BorderType => BorderType.Setting;

        internal override IMenuItemDisplayble Displayble => this;

        internal MenuItemControlDeviceSetting(Texture2D gamePad, Texture2D keyboard)
        {
            sprites = new Dictionary<ControlDevice, Texture2D>
            {
                { ControlDevice.GamePad, gamePad },
                { ControlDevice.Keyboard, keyboard }
            };
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprites[SelectedDevice], position, Color.White);
        }

        public Point GetSize()
        {
            var sprite = sprites[SelectedDevice];
            return new Point(sprite.Width, sprite.Height);
        }

        internal override void RequestCommandExecution()
        {
            ChangeDevice();
        }

        internal override void Decrement()
        {
            ChangeDevice();
        }

        internal override void Increment()
        {
            ChangeDevice();
        }

        private void ChangeDevice()
        {
            if (SelectedDevice == ControlDevice.GamePad)
                SelectedDevice = ControlDevice.Keyboard;
            else
                SelectedDevice = ControlDevice.GamePad;
        }
    }
}
