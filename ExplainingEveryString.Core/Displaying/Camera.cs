using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying
{
    internal class Camera
    {
        private AssetsStorage AssetsStorage => game.AssetsStorage;
        private SpriteBatch spriteBatch;       
        private Single screenHeight;
        private CameraObjectGlass objectGlass;
        private EesGame game;

        internal Vector2 PlayerPositionOnScreen => objectGlass.PlayerPositionOnScreen;

        internal Camera(Level level, EesGame game, Configuration config)
        {
            this.game = game;
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.screenHeight = game.GraphicsDevice.Viewport.Height;
            this.objectGlass = new CameraObjectGlass(level, game.GraphicsDevice, 
                config.PlayerFramePercentageWidth, config.PlayerFramePercentageHeigth);
        }

        internal void Begin() => spriteBatch.Begin();

        internal void End() => spriteBatch.End();

        internal void Draw(IEnumerable<IDisplayble> thingsToDraw)
        {
            foreach (IDisplayble toDraw in thingsToDraw)
            {
                Draw(toDraw);
                if (toDraw is IMultiPartDisplayble)
                {
                    foreach (IDisplayble part in ((IMultiPartDisplayble)toDraw).GetParts())
                        Draw(part);
                }
            }   
        }

        private void Draw(IDisplayble toDraw)
        {
            if (!toDraw.IsVisible)
                return;

            SpriteState spriteState = toDraw.SpriteState;
            SpriteData spriteData = AssetsStorage.GetSprite(spriteState.Name);
            Vector2 position = toDraw.Position;
            Vector2 drawPosition = GetDrawPosition(position, spriteData);
            Rectangle? drawPart = GetDrawPart(spriteData, spriteState);
            Single angle = -spriteState.Angle;
            Vector2 spriteCenter = new Vector2
            {
                X = spriteData.Sprite.Width / spriteData.AnimationFrames / 2,
                Y = spriteData.Sprite.Height / 2
            };

            spriteBatch.Draw(spriteData.Sprite, drawPosition, drawPart, Color.White, angle, 
                spriteCenter, 1, SpriteEffects.None, 0);
        }

        private Vector2 GetDrawPosition(Vector2 position, SpriteData spriteData)
        {
            Texture2D sprite = spriteData.Sprite;
            Vector2 cameraOffset = objectGlass.CameraOffset;
            Vector2 centerOfSpriteOnScreen = position - cameraOffset;
            centerOfSpriteOnScreen.Y = screenHeight - centerOfSpriteOnScreen.Y;
            return centerOfSpriteOnScreen;
        }

        private Rectangle? GetDrawPart(SpriteData spriteData, SpriteState spriteState)
        {
            if (spriteData.AnimationFrames == 1)
                return null;

            Int32 frameWidth = spriteData.Sprite.Width / spriteData.AnimationFrames;
            Single frameTime = spriteState.AnimationCycle / spriteData.AnimationFrames;
            Int32 globalFrameNumber = (Int32)(spriteState.ElapsedTime / frameTime);
            Int32 frameNumber = globalFrameNumber % spriteData.AnimationFrames;
            return new Rectangle {
                X = frameNumber * frameWidth,
                Y = 0,
                Width = frameWidth,
                Height = spriteData.Sprite.Height
            };
        
        }

        internal void Update()
        {
            objectGlass.Update();
        }
    }
}
