﻿using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core.Menu
{
    internal class InnerMenuInputProcessor : MenuInputProcessor
    {
        protected override MenuButtonHandler[] Buttons { get; set; } = new MenuButtonHandler[6];
        internal MenuButtonHandler Up { get => Buttons[0]; private set => Buttons[0] = value; }
        internal MenuButtonHandler Down { get => Buttons[1]; private set => Buttons[1] = value; }
        internal MenuButtonHandler Accept { get => Buttons[2]; private set => Buttons[2] = value; }
        internal MenuButtonHandler Back { get => Buttons[3]; private set => Buttons[3] = value; }
        internal MenuButtonHandler Left { get => Buttons[4]; private set => Buttons[4] = value; }
        internal MenuButtonHandler Right { get => Buttons[5]; private set => Buttons[5] = value; }

        internal InnerMenuInputProcessor(Configuration configuration) : base(configuration) { }

        protected override void InitGamepadButtons()
        {
            Up = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed);
            Down = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed);
            Left = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed);
            Right = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed);
            Accept = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed);
            Back = new MenuButtonHandler(() => GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed);
        }

        protected override void InitKeyboardButtons()
        {
            Up = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.W));
            Down = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.S));
            Left = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.A));
            Right = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.D));
            Accept = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Space));
            Back = new MenuButtonHandler(() => Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.Escape));
        }
    }
}
