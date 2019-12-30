using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemDisplayer
    {
        private Texture2D borderPart;
        private SpriteBatch spriteBatch;

        internal MenuItemDisplayer(Texture2D borderPart, SpriteBatch spriteBatch)
        {
            this.borderPart = borderPart;
            this.spriteBatch = spriteBatch;
        }

        internal void Draw(MenuItem item, Point pointPosition)
        {
            var position = new Vector2(pointPosition.X, pointPosition.Y);
            spriteBatch.Draw(item.Sprite, position, Color.White);
            if (item.Selected)
                DrawBorder(item, pointPosition);
        }

        internal void DrawBorder(MenuItem item, Point pointPosition)
        {
            DrawHorizontallBorder(
                pointPosition.X - borderPart.Width,
                pointPosition.X + item.Sprite.Width + borderPart.Width,
                pointPosition.Y - borderPart.Height);
            DrawHorizontallBorder(
                pointPosition.X - borderPart.Width,
                pointPosition.X + item.Sprite.Width + borderPart.Width,
                pointPosition.Y + item.Sprite.Height);
            DrawVerticalBorder(
                pointPosition.Y,
                pointPosition.Y + item.Sprite.Height,
                pointPosition.X - borderPart.Width);
            DrawVerticalBorder(
                pointPosition.Y,
                pointPosition.Y + item.Sprite.Height,
                pointPosition.X + item.Sprite.Width);
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
