using ExplainingEveryString.Data.AssetsMetadata;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class SpriteDataBuilder
    {
        private ContentManager contentManager;

        internal SpriteDataBuilder(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        internal Dictionary<String, SpriteData> Build(IEnumerable<String> sprites, IAssetsMetadataLoader metadataLoader)
        {
            Dictionary<String, SpriteData> spritesData = new Dictionary<string, SpriteData>();
            foreach (String spriteName in sprites)
            {
                spritesData[spriteName] = new SpriteData
                {
                    Sprite = contentManager.Load<Texture2D>(spriteName),
                    AnimationFrames = 1
                };
            }
            AddMetadata(spritesData, metadataLoader);
            return spritesData;
        }

        private void AddMetadata(Dictionary<String, SpriteData> spritesData, IAssetsMetadataLoader metadataLoader)
        {
            AssetsMetadata assetsMetadata = metadataLoader.Load();
            foreach (SpriteMetadata spriteMetadata in assetsMetadata.SpritesMetadata.Where(m => spritesData.ContainsKey(m.Name)))
            {
                spritesData[spriteMetadata.Name].AnimationFrames = spriteMetadata.AnimationFrames;
                spritesData[spriteMetadata.Name].DefaultAnimationCycle = spriteMetadata.DefaultAnimationCycle;
            }
        }
    }
}
