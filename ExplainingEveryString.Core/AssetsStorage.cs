using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core
{
    internal interface IAssetsStorage
    {
        SpriteData GetSprite(String name);
        SoundEffect GetSound(String name);
    }

    internal class AssetsStorage : IAssetsStorage
    {
        private Dictionary<String, SpriteData> spritesStorage = new Dictionary<String, SpriteData>();
        private Dictionary<String, SoundEffect> soundsStorage = new Dictionary<String, SoundEffect>();

        internal void FillAssetsStorages(IBlueprintsLoader blueprintsLoader, SpriteEmitterData spriteEmitterData,
            IAssetsMetadataLoader metadataLoader, ContentManager contentManager)
        {
            var spriteDataBuilder = new SpriteDataBuilder(contentManager, metadataLoader);
            var sprites = AssetsExtractor.GetNeccessarySprites(blueprintsLoader);
            if (spriteEmitterData != null)
            {
                var spritesList = spriteEmitterData.RandomSprites.PossibleValues.Select(ss => ss.Name).Distinct();
                sprites = sprites.Concat(spritesList).ToList();
            }
            spritesStorage = spriteDataBuilder.Build(sprites);

            foreach (var soundName in AssetsExtractor.GetNecessarySounds(blueprintsLoader))
            {
                soundsStorage[soundName] = contentManager.Load<SoundEffect>(soundName);
            }
        }

        public SpriteData GetSprite(String name)
        {
            return spritesStorage[name];
        }

        public SoundEffect GetSound(String name)
        {
            return soundsStorage[name];
        }
    }
}
