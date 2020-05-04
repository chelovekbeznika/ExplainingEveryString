using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
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
    internal class BlueprintDisplayer : IEditableDisplayer
    {
        private Dictionary<String, Texture2D> sprites = new Dictionary<String, Texture2D>();
        private Dictionary<String, SpriteMetadata> spritesMetadata = new Dictionary<String, SpriteMetadata>();

        /// <summary>
        /// Better call this in LoadContent. Just in case.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="blueprintsLoader"></param>
        /// <param name="assetsMetadata"></param>
        internal BlueprintDisplayer(ContentManager content, IBlueprintsLoader blueprintsLoader, AssetsMetadata assetsMetadata)
        {
            var blueprints = blueprintsLoader.GetBlueprints();
            foreach (var pair in blueprints)
            {
                var type = pair.Key;
                var blueprint = pair.Value;
                sprites.Add(type, content.Load<Texture2D>(blueprint.DefaultSprite.Name));
                var spriteMetadata = assetsMetadata.SpritesMetadata.FirstOrDefault(m => m.Name == blueprint.DefaultSprite.Name);
                if (spriteMetadata != null)
                    spritesMetadata.Add(type, spriteMetadata);
            }
        }

        public void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen)
        {
            var texture = sprites[type];
            var metadata = spritesMetadata[type];
            var frames = metadata != null ? metadata.AnimationFrames : 1;
            var frame = metadata != null ? metadata.FrameToShowInEditor : 0;
            var width = texture.Width / frames;
            var partToDraw = new Rectangle { X = frame * width, Y = 0, Width = width, Height = texture.Height };
            var centerOfSprite = new Vector2(width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, positionOnScreen, partToDraw, Color.White, 
                rotation: 0, origin: centerOfSprite, scale: 1, effects: SpriteEffects.None, layerDepth: 0);
        }
    }
}
