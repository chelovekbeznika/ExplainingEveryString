using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Data.Notifications;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Notifications
{
    internal class NotificationsAssetsStorage : IAssetsStorage
    {
        private Dictionary<String, SpriteData> spritesStorage = new Dictionary<String, SpriteData>();
        private Dictionary<String, SoundEffect> soundsStorage = new Dictionary<String, SoundEffect>();

        internal void FillStorage(Dictionary<String, NotificationSpecification> notificationsSpecs, SpriteDataBuilder builder, ContentManager content)
        {
            var spriteNames = notificationsSpecs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.SpriteName);
            var rawSprites = builder.Build(spriteNames.Values);
            foreach (var kvp in spriteNames)
            {
                spritesStorage.Add(kvp.Key, rawSprites[kvp.Value]);
            }

            foreach (var kvp in notificationsSpecs)
            {
                soundsStorage.Add(kvp.Key, content.Load<SoundEffect>(kvp.Value.SoundName));
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
