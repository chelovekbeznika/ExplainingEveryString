using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core.Menu
{
    internal class OuterMenuInputProcessor : MenuInputProcessor
    {
        protected override MenuButtonHandler[] Buttons { get; set; } = new MenuButtonHandler[4];

        internal MenuButtonHandler Pause { get => Buttons[0]; private set => Buttons[0] = value; }
        internal MenuButtonHandler FirstProfile { get => Buttons[1]; private set => Buttons[1] = value; }
        internal MenuButtonHandler SecondProfile { get => Buttons[2]; private set => Buttons[2] = value; }
        internal MenuButtonHandler ThirdProfile { get => Buttons[3]; private set => Buttons[3] = value; }

        internal OuterMenuInputProcessor() : base() { }

        protected override void InitButtons()
        {
            Pause = new MenuButtonHandler(() => 
                GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape));
            FirstProfile = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.F1));
            SecondProfile = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.F2));
            ThirdProfile = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.F3));
        }
    }
}
