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
        internal event EventHandler OnPause;
        internal event EventHandler OnExit;
        
        private Func<Boolean> pauseButtonPressed;
        private Func<Boolean> exitButtonPressed;

        internal MenuInputProcessor(Configuration config)
        {
            switch (config.ControlDevice)
            {
                case ControlDevice.GamePad:
                    pauseButtonPressed = () => GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed;
                    exitButtonPressed = () => GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
                    break;
                case ControlDevice.Keyboard:
                    pauseButtonPressed = () => Keyboard.GetState().IsKeyDown(Keys.Space);
                    exitButtonPressed = () => Keyboard.GetState().IsKeyDown(Keys.Escape);
                    break;
            }
        }

        internal void Update()
        {
            if (pauseButtonPressed())
                OnPause(this, EventArgs.Empty);
            if (exitButtonPressed())
                OnExit(this, EventArgs.Empty);
        }
    }
}
