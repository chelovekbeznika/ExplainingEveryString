using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core
{
    internal class Mine : GameObject
    {
        internal const String CommonSpriteName = "mine";

        internal Mine(Vector2 position) : base(CommonSpriteName, position) { }
    }
}
