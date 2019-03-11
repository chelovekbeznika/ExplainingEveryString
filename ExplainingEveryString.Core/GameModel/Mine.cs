using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Mine : GameObject
    {
        internal const String CommonSpriteName = "mine";

        internal Mine(Vector2 position) : base(CommonSpriteName, position) { }
    }
}
