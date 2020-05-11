using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class RectangleCornersDisplayer : IEditableDisplayer
    {
        private Texture2D corner;

        internal RectangleCornersDisplayer(ContentManager content)
        {
            corner = content.Load<Texture2D>(@"Sprites/Editor/Corner");
        }

        public void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen, Boolean selected)
        {
            var centerOfSprite = new Vector2(corner.Width / 2, corner.Height / 2);
            spriteBatch.Draw(corner, positionOnScreen, null, selected ? Color.White : Color.Blue,
                rotation: 0, origin: centerOfSprite, scale: 1, effects: SpriteEffects.None, layerDepth: 0);
        }
    }
}
