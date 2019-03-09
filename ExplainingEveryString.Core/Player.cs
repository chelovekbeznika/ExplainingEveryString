using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal sealed class Player : GameObject
    {
        private const String spriteName = "player";
        private readonly Int32 speed = 100;

        internal Player(EesGame game) : base(game, spriteName, new Vector2(0,0)) { }

        internal void Move(GameTime gameTime)
        {
            IPlayerInput playerInput = PlayerInputFactory.Create();
            Vector2 direction = playerInput.GetMoveDirection();
            Vector2 positionChange = direction * speed * (Single)gameTime.ElapsedGameTime.TotalSeconds;
            Position += positionChange;
        }
    }
}
