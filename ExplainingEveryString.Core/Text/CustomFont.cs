using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.Text
{
    internal abstract class CustomFont
    {
        protected Dictionary<Char, Texture2D> chars;
        protected abstract Int32 BetweenChars { get; }

        internal abstract void Load(ContentManager content);

        internal Point GetSize(String text)
        {
            var width = text.Select(c => chars[c].Width).Sum() + BetweenChars * (text.Length - 1);
            var height = text.Select(c => chars[c].Height).Max();
            return new Point(width, height);
        }

        internal void Draw(SpriteBatch spriteBatch, Vector2 position, String text)
        {
            var x = position.X;
            var height = text.Select(c => chars[c].Height).Max();
            foreach (var c in text)
            {
                var y = position.Y + height - chars[c].Height;
                spriteBatch.Draw(chars[c], new Vector2(x, y), Color.White);
                x += chars[c].Width + BetweenChars;
            }
        }
    }
}
