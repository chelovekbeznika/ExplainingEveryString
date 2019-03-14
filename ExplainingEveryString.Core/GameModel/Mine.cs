using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Mine : GameObject
    {
        internal const String CommonSpriteName = "mine";

        protected override Single Height => 16;
        protected override Single Width => 16;

        internal Mine(Vector2 position) : base(CommonSpriteName, position) { }
    }
}
