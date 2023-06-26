using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Text
{
    internal abstract class CustomFont
    {
        protected Dictionary<Char, Texture2D> Chars { get; set; }
        protected abstract Int32 BetweenChars { get; }

        internal abstract void Load(ContentManager content);

        internal Point GetSize(String text)
        {
            var width = text.Select(c => Chars[c].Width).Sum() + BetweenChars * (text.Length - 1);
            var height = text.Select(c => Chars[c].Height).Max();
            return new Point(width, height);
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, String text)
        {
            Draw(spriteBatch, position, text, Color.White);
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, String text, Color colorMask)
        {
            var x = position.X;
            var height = text.Select(c => Chars[c].Height).Max();
            foreach (var c in text)
            {
                var y = position.Y + height - Chars[c].Height;
                spriteBatch.Draw(Chars[c], new Vector2(x, y), colorMask);
                x += Chars[c].Width + BetweenChars;
            }
        }
    }
}
