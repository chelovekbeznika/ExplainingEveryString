using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
            foreach (String spriteName in blueprintsLoader.GetNeccessarySprites())
            {
                spritesStorage[spriteName] = new SpriteData
                {
                    Sprite = contentManager.Load<Texture2D>(spriteName),
                    AnimationFrames = 1
                };  
            }
            foreach (String soundName in blueprintsLoader.GetNecessarySounds())
            {
                soundsStorage[soundName] = contentManager.Load<SoundEffect>(soundName);
            }
            AssetsMetadata assetsMetadata = metadataLoader.Load();
            foreach (SpriteMetadata spriteMetadata in assetsMetadata.SpritesMetadata)
            {
                spritesStorage[spriteMetadata.Name].AnimationFrames = spriteMetadata.AnimationFrames;
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
