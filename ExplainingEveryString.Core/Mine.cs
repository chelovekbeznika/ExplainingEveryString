using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core
{
    internal class Mine : GameObject
    {
        private const String spriteName = "mine";

        internal Mine(EesGame game, Vector2 position) : base(game, spriteName, position) { }
    }
}
