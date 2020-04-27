using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class KeyboardInputProcessor : IUpdateable
    {
        private Keys[] pressedAtPreviousFrame = Array.Empty<Keys>();

        internal event EventHandler<KeyPressedEventArgs> KeyPressed;

        public void Update(Single elapsedSeconds)
        {
            var pressedCurrently = Keyboard.GetState().GetPressedKeys();
            var released = pressedCurrently.Except(pressedAtPreviousFrame);

            foreach (var releasedKey in released)
            {
                KeyPressed?.Invoke(this, new KeyPressedEventArgs { PressedKey = releasedKey });
            }

            pressedAtPreviousFrame = pressedCurrently;
        }
    }

    internal class KeyPressedEventArgs : EventArgs
    {
        internal Keys PressedKey { get; set; }
    }
}
