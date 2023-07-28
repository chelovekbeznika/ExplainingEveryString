using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Text
{
    internal class MainMenuFont : CustomFont
    {
        protected override Int32 BetweenChars => 8;

        internal override void Load(ContentManager content)
        {
            Chars = new Dictionary<Char, Texture2D>
            {
                { ' ', content.Load<Texture2D>(@"Sprites/Fonts/MainMenu/Space") },
                { '.', content.Load<Texture2D>(@"Sprites/Fonts/MainMenu/Dot") },
                { ',', content.Load<Texture2D>(@"Sprites/Fonts/MainMenu/Dot") },
                { ':', content.Load<Texture2D>(@"Sprites/Fonts/MainMenu/Colon") }
            };
            foreach (var c in "CONTIUELADSWGMVXK0123456789")
            {
                Chars.Add(c, content.Load<Texture2D>($@"Sprites/Fonts/MainMenu/{c}"));
            }
        }
    }
}
