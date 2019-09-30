using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal abstract class MenuInputProcessor
    {
        protected abstract MenuButtonHandler[] Buttons { get; set; }

        internal MenuInputProcessor(Configuration config)
        {
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad:
                    InitGamepadButtons();
                    break;
                case ControlDevice.Keyboard:
                    InitKeyboardButtons();
                    break;
            }
        }

        internal void Update(GameTime gameTime)
        {
            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (MenuButtonHandler button in Buttons)
                button.Update(elapsedSeconds);
        }

        protected abstract void InitGamepadButtons();
        protected abstract void InitKeyboardButtons();
    }
}
