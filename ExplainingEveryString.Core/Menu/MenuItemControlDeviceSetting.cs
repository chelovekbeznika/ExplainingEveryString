using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemControlDeviceSetting : MenuItem
    {
        private Dictionary<ControlDevice, Texture2D> sprites;
        private ControlDevice SelectedDevice 
        { 
            get => SettingsAccess.GetCurrentSettings().PreferrableControlDevice;
            set => SettingsAccess.GetCurrentSettings().PreferrableControlDevice = value;
        }

        internal override BorderType BorderType => BorderType.Setting;

        internal MenuItemControlDeviceSetting(Texture2D gamePad, Texture2D keyboard)
        {
            sprites = new Dictionary<ControlDevice, Texture2D>
            {
                { ControlDevice.GamePad, gamePad },
                { ControlDevice.Keyboard, keyboard }
            };
        }

        internal override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(sprites[SelectedDevice], position, Color.White);
        }

        internal override Point GetSize()
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
