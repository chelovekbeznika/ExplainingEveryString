using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class InputProcessor : IUpdateable
    {
        internal static readonly InputProcessor Instance = new InputProcessor();

        private Keys[] pressedAtPreviousFrame = Array.Empty<Keys>();
        private Int32 scroolAtPreviousFrame = Mouse.GetState().ScrollWheelValue;

        internal event EventHandler<KeyPressedEventArgs> KeyPressed;
        internal event EventHandler<MouseScrolledEventArgs> MouseScrolled;

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
        }
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
