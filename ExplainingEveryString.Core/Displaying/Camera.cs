using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying
{
    internal class Camera
    {
        private AssetsStorage AssetsStorage => game.AssetsStorage;     
        private readonly Single screenHeight;
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private EesGame game;

        internal Vector2 PlayerPositionOnScreen => screenCoordinatesMaster.PlayerPosition;

        internal Camera(Level level, EesGame game, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            this.game = game;
            this.screenHeight = game.GraphicsDevice.Viewport.Height;
            this.screenCoordinatesMaster = screenCoordinatesMaster;
        }

        internal void Draw(SpriteBatch spriteBatch, IEnumerable<IDisplayble> thingsToDraw)
        {
            foreach (var toDraw in thingsToDraw)
            {
                Draw(spriteBatch, toDraw);
            }   
        }

        private void Draw(SpriteBatch spriteBatch, IDisplayble toDraw)
        {
            if (!toDraw.IsVisible)
                return;

            var spriteState = toDraw.SpriteState;
            var spriteData = AssetsStorage.GetSprite(spriteState.Name);
            var position = toDraw.Position;
            var drawPosition = screenCoordinatesMaster.ConvertToScreenPosition(position);
            var drawPart = AnimationHelp.GetDrawPart(spriteData, spriteState.AnimationCycle, spriteState.ElapsedTime);
            var angle = -spriteState.Angle;
            var spriteCenter = new Vector2
            {
                X = spriteData.Width / 2,
                Y = spriteData.Height / 2
            };

            spriteBatch.Draw(spriteData.Sprite, drawPosition, drawPart, Color.White, angle, 
                spriteCenter, 1, SpriteEffects.None, 0);

            foreach (var part in toDraw.GetParts())
                Draw(spriteBatch, part);
        }

        internal void Update(Single elapsedSeconds)
        {
            screenCoordinatesMaster.Update(elapsedSeconds);
        }

        internal Rectangle PositionOnScreen(IDisplayble displayble)
        {
            var sprite = AssetsStorage.GetSprite(displayble.SpriteState.Name);
            return screenCoordinatesMaster.PositionOnScreen(displayble.Position, sprite);
        }

        internal Boolean IsVisibleOnScreen(IDisplayble displayble)
        {
            var sprite = AssetsStorage.GetSprite(displayble.SpriteState.Name);
            return screenCoordinatesMaster.IsVisibleOnScreen(displayble.Position, sprite);
        }
    }
}
