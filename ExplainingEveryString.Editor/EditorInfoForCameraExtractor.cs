using ExplainingEveryString.Core;
using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class EditorInfoForCameraExtractor : IMainCharacterInfoForCameraExtractor
    {
        private Vector2 position;

        internal EditorInfoForCameraExtractor(Vector2 startPosition)
        {
            position = startPosition;
        }

        public Vector2 Position => position;

        public Vector2 FireDirection => Vector2.UnitX;

        public Single Focused => 0;
    }
}
