using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : GameObject
    {
        internal const String CommonSpriteName = "player";

        private readonly Int32 speed = 400;

        protected override Single Height => 32;
        protected override Single Width => 32;

        internal Player() : base(CommonSpriteName, new Vector2(0,0)) { }

        internal void Move(Single elapsedSeconds)
        {
            IPlayerInput playerInput = PlayerInputFactory.Create();
            Vector2 direction = playerInput.GetMoveDirection();
            Vector2 positionChange = direction * speed * elapsedSeconds;
            Position += positionChange;
        }
    }
}
