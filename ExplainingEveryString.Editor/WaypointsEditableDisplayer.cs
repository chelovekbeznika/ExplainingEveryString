using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Editor
{
    internal class WaypointsEditableDisplayer : IEditableDisplayer
    {
        private Texture2D sprite;
        private Texture2D selectedSprite;
        private Dictionary<String, Color> colors = new Dictionary<String, Color>
        {
            { "Alpha", Color.White },
            { "Beta", Color.Aquamarine },
            { "Gamma", Color.Beige },
            { "Delta", Color.Brown },
            { "Epsilon", Color.Black },
            { "Dzeta", Color.Crimson },
            { "Eta", Color.Cyan },
            { "Teta", Color.DarkOrange },
            { "Yota", Color.DeepSkyBlue },
            { "Kappa", Color.ForestGreen },
            { "Lambda", Color.Gray },
            { "Mu", Color.Gold },
            { "Nu", Color.Green },
            { "Xi", Color.Magenta },
            { "Omicron", Color.Orange },
            { "Pi", Color.Indigo },
            { "Ro", Color.Pink },
            { "Sigma", Color.Lime },
            { "Tau", Color.Violet },
            { "Ypsilon", Color.Purple },
            { "Fi", Color.MintCream },
            { "Hi", Color.Khaki },
            { "Psi", Color.Salmon },
            { "Omega", Color.Chocolate }
        };

        internal WaypointsEditableDisplayer(ContentManager content)
        {
            this.selectedSprite = content.Load<Texture2D>(@"Sprites/Editor/SelectedWaypoint");
            this.sprite = content.Load<Texture2D>(@"Sprites/Editor/Waypoint");
        }

        public void Draw(SpriteBatch spriteBatch, string type, Vector2 positionOnScreen, bool selected)
        {
            var centerOfSprite = new Vector2(sprite.Width / 2, sprite.Height / 2);
            spriteBatch.Draw(selected ? selectedSprite : sprite, positionOnScreen, null, colors[type],
                rotation: 0, origin: centerOfSprite, scale: 1, effects: SpriteEffects.None, layerDepth: 0);
        }
    }
}
