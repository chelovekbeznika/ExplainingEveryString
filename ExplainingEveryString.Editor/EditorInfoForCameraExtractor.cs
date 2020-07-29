using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Editor
{
    internal class EditorInfoForCameraExtractor : IMainCharacterInfoForCameraExtractor
    {
        private Vector2 position;
        private const Single step = 16 * 16;

        public Vector2 Position => position;

        public Vector2 FireDirection => Vector2.UnitX;

        public Single Focused => 0;

        internal EditorInfoForCameraExtractor(Vector2 startPosition)
        {
            position = startPosition;
            InputProcessor.Instance.KeyPressed += KeyPressed;
        }

        private void KeyPressed(Object sender, KeyPressedEventArgs e)
        {
            switch (e.PressedKey)
            {
                case Keys.W: position += new Vector2(0, step); break;
                case Keys.S: position += new Vector2(0, -step); break;
                case Keys.A: position += new Vector2(-step, 0); break;
                case Keys.D: position += new Vector2(step, 0); break;
            }
        }
    }
}
