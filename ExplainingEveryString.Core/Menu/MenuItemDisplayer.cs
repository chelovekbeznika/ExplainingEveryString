using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemDisplayer
    {
        private const Int32 betweenPixels = 2;
        private readonly Texture2D borderPart;
        private readonly Texture2D left;
        private readonly Texture2D right;
        private readonly SpriteBatch spriteBatch;

        internal MenuItemDisplayer(SpriteBatch spriteBatch, Texture2D borderPart, Texture2D left, Texture2D right)
        {
            this.borderPart = borderPart;
            this.spriteBatch = spriteBatch;
            this.left = left;
            this.right = right;
        }

        internal void Draw(MenuItem item, Point pointPosition)
        {
            var position = new Vector2(pointPosition.X, pointPosition.Y);
            item.Displayble.Draw(spriteBatch, position);
            if (item.Selected)
            {
                var size = item.Displayble.GetSize();
                switch (item.BorderType)
                {
                    case BorderType.Button:
                        DrawButtonBorder(pointPosition, size);
                        break;
                    case BorderType.Setting:
                        DrawSettingBorder(pointPosition, size);
                        break;
                }
            }
        }

        internal void DrawSettingBorder(Point pointPosition, Point size)
        {
            var leftPosition = new Vector2(
                x: pointPosition.X - betweenPixels - left.Width,
                y: pointPosition.Y + (Single)size.Y / 2 - (Single)left.Width / 2);
            var rightPosition = new Vector2(
                x: pointPosition.X + size.X + betweenPixels,
                y: pointPosition.Y + (Single)size.Y / 2 - (Single)left.Width / 2);
            spriteBatch.Draw(left, leftPosition, Color.White);
            spriteBatch.Draw(right, rightPosition, Color.White);
        }

        internal void DrawButtonBorder(Point pointPosition, Point size)
        {
            DrawHorizontallBorder(
                pointPosition.X - borderPart.Width,
                pointPosition.X + size.X + borderPart.Width,
                pointPosition.Y - borderPart.Height);
            DrawHorizontallBorder(
                pointPosition.X - borderPart.Width,
                pointPosition.X + size.X + borderPart.Width,
                pointPosition.Y + size.Y);
            DrawVerticalBorder(
                pointPosition.Y,
                pointPosition.Y + size.Y,
                pointPosition.X - borderPart.Width);
            DrawVerticalBorder(
                pointPosition.Y,
                pointPosition.Y + size.Y,
                pointPosition.X + size.X);
        }

        internal void DrawHorizontallBorder(Int32 from, Int32 to, Int32 yLineCoord)
        {
            var currentX = from;
            while (currentX < to)
            {
                spriteBatch.Draw(borderPart, new Vector2(currentX, yLineCoord), Color.White);
                currentX += borderPart.Width;
            }
        }

        internal void DrawVerticalBorder(Int32 from, Int32 to, Int32 xLineCoord)
        {
            var currentY = from;
            while (currentY < to)
            {
                spriteBatch.Draw(borderPart, new Vector2(xLineCoord, currentY), Color.White);
                currentY += borderPart.Height;
            }
        }
    }
}
