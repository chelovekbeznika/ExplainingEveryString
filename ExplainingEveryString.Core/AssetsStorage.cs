using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core
{
    internal class AssetsStorage
    {
        private Dictionary<String, SpriteData> spritesStorage = new Dictionary<String, SpriteData>();
        private Dictionary<String, SoundEffect> soundsStorage = new Dictionary<String, SoundEffect>();

        internal void FillAssetsStorages(IBlueprintsLoader blueprintsLoader, 
            IAssetsMetadataLoader metadataLoader, ContentManager contentManager)
        {
            SpriteDataBuilder spriteDataBuilder = new SpriteDataBuilder(contentManager, metadataLoader);
            IEnumerable<String> sprites = AssetsExtractor.GetNeccessarySprites(blueprintsLoader);
            spritesStorage = spriteDataBuilder.Build(sprites);
            foreach (String soundName in AssetsExtractor.GetNecessarySounds(blueprintsLoader))
            {
                soundsStorage[soundName] = contentManager.Load<SoundEffect>(soundName);
            }
        }

        internal SpriteData GetSprite(String name)
        {
            return spritesStorage[name];
        }

        internal SoundEffect GetSound(String name)
        {
            return soundsStorage[name];
        }
    }
}
