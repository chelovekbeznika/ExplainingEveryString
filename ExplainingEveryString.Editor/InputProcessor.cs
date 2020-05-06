using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class InputProcessor : Core.GameModel.IUpdateable
    {
        internal static readonly InputProcessor Instance = new InputProcessor();

        private Keys[] pressedAtPreviousFrame = Array.Empty<Keys>();
        private Boolean leftPressedAtPreviousFrame = false;
        private Int32 scroolAtPreviousFrame = Mouse.GetState().ScrollWheelValue;

        internal event EventHandler<KeyPressedEventArgs> KeyPressed;
        internal event EventHandler<MouseScrolledEventArgs> MouseScrolled;
        internal event EventHandler<MouseButtonPressedEventArgs> MouseButtonPressed;

        public void Update(Single elapsedSeconds)
        {
            var pressedCurrently = Keyboard.GetState().GetPressedKeys();
            var released = pressedCurrently.Except(pressedAtPreviousFrame);
            foreach (var releasedKey in released)
            {
                KeyPressed?.Invoke(this, new KeyPressedEventArgs { PressedKey = releasedKey });
            }
            pressedAtPreviousFrame = pressedCurrently;

            var currentScroll = Mouse.GetState().ScrollWheelValue;
            if (currentScroll != scroolAtPreviousFrame)
                MouseScrolled?.Invoke(this, new MouseScrolledEventArgs { ScrollDifference = (currentScroll - scroolAtPreviousFrame) / 120 });
            scroolAtPreviousFrame = currentScroll;

            var leftPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
            if (leftPressedAtPreviousFrame && !leftPressed)
                MouseButtonPressed?.Invoke(this, new MouseButtonPressedEventArgs
                {
                    PressedButton = MouseButtons.Left,
                    MouseScreenPosition = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y)
                });
            leftPressedAtPreviousFrame = leftPressed;
        }
    }

    internal enum MouseButtons
    {
        Left
    }

    internal class MouseButtonPressedEventArgs : EventArgs
    {
        internal MouseButtons PressedButton { get; set; }
        internal Vector2 MouseScreenPosition { get; set; }
    }

    internal class KeyPressedEventArgs : EventArgs
    {
        internal Keys PressedKey { get; set; }
    }

    internal class MouseScrolledEventArgs : EventArgs
    {
        internal Int32 ScrollDifference { get; set; }
    }
}
