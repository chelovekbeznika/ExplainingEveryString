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
    internal class OuterMenuInputProcessor : MenuInputProcessor
    {
        protected override MenuButtonHandler[] Buttons { get; set; } = new MenuButtonHandler[1];

        internal MenuButtonHandler Pause { get => Buttons[0]; private set => Buttons[0] = value; }

        internal OuterMenuInputProcessor(Configuration configuration) : base(configuration) { }

        protected override void InitGamepadButtons()
        {
            Pause = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed);
        }

        protected override void InitKeyboardButtons()
        {
            Pause = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Escape));
        }
    }
}
