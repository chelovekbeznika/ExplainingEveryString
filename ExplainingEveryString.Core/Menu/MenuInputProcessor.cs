using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuInputProcessor
    {
        internal MenuButtonHandler Pause { get; private set; }
        internal MenuButtonHandler Exit { get; private set; }

        internal MenuInputProcessor(Configuration config)
        {
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad:
                    Pause = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed);
                    Exit = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed);
                    break;
                case ControlDevice.Keyboard:
                    Pause = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Space));
                    Exit = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Escape));
                    break;
            }
        }

        internal void Update(Single elapsedSeconds)
        {
            Pause.Update(elapsedSeconds);
            Exit.Update(elapsedSeconds);
        }

        private void InitGamepadButtons()
        {
            Pause = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed);
            Exit = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed);
        }

        private void InitKeyboardButtons()
        {
            Pause = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Space));
            Exit = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Escape));
        }
    }
}
