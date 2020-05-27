using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Editor
{
    internal abstract class SpriteDisplayer : IEditableDisplayer
    {
        private Texture2D sprite;

        protected abstract Color SelectedColor { get; }
        protected abstract Color UnselectedColor { get; }

        protected SpriteDisplayer(ContentManager content, String texturePath)
        {
            this.sprite = content.Load<Texture2D>(texturePath);
        }

        public void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen, Boolean selected)
        {
            var centerOfSprite = new Vector2(sprite.Width / 2, sprite.Height / 2);
            spriteBatch.Draw(sprite, positionOnScreen, null, selected ? SelectedColor : UnselectedColor,
                rotation: 0, origin: centerOfSprite, scale: 1, effects: SpriteEffects.None, layerDepth: 0);
        }
    }

    internal class RectangleCornersDisplayer : SpriteDisplayer
    {
        internal RectangleCornersDisplayer(ContentManager content) : base(content, @"Sprites/Editor/Corner") { }

        protected override Color SelectedColor => Color.Blue;

        protected override Color UnselectedColor => Color.White;
    }

    internal class SpawnPointsDisplayer : SpriteDisplayer
    {
        internal SpawnPointsDisplayer(ContentManager content) : base(content, @"Sprites/Editor/SpawnPointMarkers/Base") { }

        protected override Color SelectedColor => Color.Black;

        protected override Color UnselectedColor => Color.White;
    }
}
