using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core.Menu
{
    internal class OuterMenuInputProcessor : MenuInputProcessor
    {
        protected override MenuButtonHandler[] Buttons { get; set; } = new MenuButtonHandler[1];

        internal MenuButtonHandler Pause { get => Buttons[0]; private set => Buttons[0] = value; }

        internal OuterMenuInputProcessor() : base() { }

        protected override void InitButtons()
        {
            Pause = new MenuButtonHandler(() => 
                GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape));
        }
    }
}
