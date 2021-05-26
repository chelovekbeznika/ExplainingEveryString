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
            item.Draw(spriteBatch, position);
            if (item.Selected)
                DrawBorder(item, pointPosition);
        }

        internal void DrawBorder(MenuItem item, Point pointPosition)
        {
            var size = item.GetSize();
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
