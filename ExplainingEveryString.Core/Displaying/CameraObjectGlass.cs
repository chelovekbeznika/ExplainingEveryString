using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class CameraObjectGlass
    {
        private readonly Vector2 playerFrame;
        private Level level;
        private Vector2 screenHalf;
        private Single screenHeight;
        private Vector2 cameraCenter;
        private Vector2 playerPositionOnScreen { get; }
        private Single cameraMoveSpeed;
      
        internal Vector2 CameraOffset => cameraCenter - screenHalf;
        internal Vector2 PlayerPositionOnScreen
        {
            get
            {
                Vector2 result = level.PlayerPosition - cameraCenter + screenHalf;
                result.Y = screenHeight - result.Y;
                return result;
            }
        }

        internal CameraObjectGlass(Level level, GraphicsDevice graphicsDevice, 
            Single playerFramePercentageWidth, Single playerFramePercentageHeight)
        {
            Viewport viewport = graphicsDevice.Viewport;
            this.screenHalf = new Vector2 { X = viewport.Width / 2, Y = viewport.Height / 2 };
            this.screenHeight = viewport.Height;
            this.level = level;
            this.cameraCenter = level.PlayerPosition;
            this.playerFrame = new Vector2()
            {
                X = screenHalf.X * playerFramePercentageWidth / 100,
                Y = screenHalf.Y * playerFramePercentageHeight / 100
            };
            this.cameraMoveSpeed = CalculateCameraMoveSpeed(3F);
        }

        internal void Update(Single elapsedSeconds)
        {
            Vector2 desiredCenter = CalculateDesiredCenter();
            Single maxFrameCameraMove = cameraMoveSpeed * elapsedSeconds;
            if ((desiredCenter - cameraCenter).Length() < maxFrameCameraMove)
                cameraCenter = desiredCenter;
            else
            {
                Vector2 normalizedCameraMoveDirection = (desiredCenter - cameraCenter);
                normalizedCameraMoveDirection.Normalize();
                cameraCenter += normalizedCameraMoveDirection * maxFrameCameraMove;
            }
        }

        private Vector2 CalculateDesiredCenter()
        {
            Vector2 screenFrameArrow = -level.PlayerFireDirection;
            Single targetXPosition = screenFrameArrow.X < 0 ? -playerFrame.X : playerFrame.X;
            Single targetYPosition = screenFrameArrow.Y < 0 ? -playerFrame.Y : playerFrame.Y;
            Single distanceToFrameX = targetYPosition / screenFrameArrow.Y;
            Single distanceToFrameY = targetXPosition / screenFrameArrow.X;
            Vector2 desiredPlayerPositionRelativeToCenter =
                (!Single.IsInfinity(distanceToFrameX) && distanceToFrameX < distanceToFrameY) || Single.IsInfinity(distanceToFrameY)
                    ? screenFrameArrow * distanceToFrameX
                    : screenFrameArrow * distanceToFrameY;
            return level.PlayerPosition - desiredPlayerPositionRelativeToCenter;
        }

        private Single CalculateCameraMoveSpeed(Single timeFromAngleToAngle)
        {
            Single distanceFromAngleToAngle = playerFrame.Length() * 2;
            return distanceFromAngleToAngle / timeFromAngleToAngle;
        }
    }
}
