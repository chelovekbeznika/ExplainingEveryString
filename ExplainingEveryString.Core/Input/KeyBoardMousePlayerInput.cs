﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class KeyBoardMousePlayerInput : PlayerInput
    {
        private static readonly Vector2 down = new Vector2(0, -1);
        private static readonly Vector2 up = new Vector2(0, 1);
        private static readonly Vector2 left = new Vector2(-1, 0);
        private static readonly Vector2 right = new Vector2(1, 0);

        public override Vector2 GetMoveDirection()
        {
            Vector2 direction = new Vector2(0, 0);
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S))
                direction += down;
            if (keyboard.IsKeyDown(Keys.W))
                direction += up;
            if (keyboard.IsKeyDown(Keys.A))
                direction += left;
            if (keyboard.IsKeyDown(Keys.D))
                direction += right;
            return CutDirectionVector(direction);
        }

        public override Boolean IsFiring()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public override Vector2 GetFireDirection()
        {
            Vector2 playerPosition = EesGame.CurrentlyRunnedGame.Camera.PlayerPositionOnScreen;
            Point mousePoint = Mouse.GetState().Position;
            Vector2 mousePosition = new Vector2(mousePoint.X, mousePoint.Y);
            Vector2 screenDirection = mousePosition - playerPosition;
            Vector2 levelDirection = new Vector2(screenDirection.X, -screenDirection.Y);
            return NormalizeDirectionVector(levelDirection);
        }
    }
}