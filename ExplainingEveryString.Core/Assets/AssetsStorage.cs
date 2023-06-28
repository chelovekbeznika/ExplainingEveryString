using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Assets
{
    internal interface IAssetsStorage
    {
        SpriteData GetSprite(string name);
        SoundEffect GetSound(string name);
    }

    internal class AssetsStorage : IAssetsStorage
    {
        private Dictionary<string, SpriteData> spritesStorage = new Dictionary<string, SpriteData>();
        private Dictionary<string, SoundEffect> soundsStorage = new Dictionary<string, SoundEffect>();

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

        public SpriteData GetSprite(string name)
        {
            return spritesStorage[name];
        }

        public SoundEffect GetSound(string name)
        {
            return soundsStorage[name];
        }
    }
}
