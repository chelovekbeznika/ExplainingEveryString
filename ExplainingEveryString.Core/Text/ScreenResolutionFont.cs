using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Text
{
    internal class ScreenResolutionFont : CustomFont
    {
        protected override Int32 BetweenChars => 2;

        internal override void Load(ContentManager content)
        {
            Texture2D loadCharTexture(String name) => content.Load<Texture2D>($@"Sprites/Fonts/Resolution/{name}");

            chars = new Dictionary<char, Texture2D>
            {
                { '1', loadCharTexture("One") },
                { '2', loadCharTexture("Two") },
                { '3', loadCharTexture("Three") },
                { '4', loadCharTexture("Four") },
                { '5', loadCharTexture("Five") },
                { '6', loadCharTexture("Six") },
                { '7', loadCharTexture("Seven") },
                { '8', loadCharTexture("Eight") },
                { '9', loadCharTexture("Nine") },
                { '0', loadCharTexture("Zero") },
                { 'x', loadCharTexture("SmallX") },
            };
        }
    }
}
