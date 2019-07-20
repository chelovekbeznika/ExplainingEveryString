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
        private MenuButtonHandler[] buttons = new MenuButtonHandler[4];
        internal MenuButtonHandler Pause { get => buttons[0]; private set => buttons[0] = value; }
        internal MenuButtonHandler Up { get => buttons[1]; private set => buttons[1] = value; }
        internal MenuButtonHandler Down { get => buttons[2]; private set => buttons[2] = value; }
        internal MenuButtonHandler Accept { get => buttons[3]; private set => buttons[3] = value; }

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

        internal void Update(Single elapsedSeconds)
        {
            foreach (MenuButtonHandler button in buttons)
                button.Update(elapsedSeconds);
        }

        private void InitGamepadButtons()
        {
            Pause = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed);
            Up = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed);
            Down = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed);
            Accept = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed);
        }

        private void InitKeyboardButtons()
        {
            Pause = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Escape));
            Up = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.W));
            Down = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.S));
            Accept = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Space));
        }
    }
}
