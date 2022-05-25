using ExplainingEveryString.Core.Assets;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal static class TextureLoadingHelper
    {
        internal static SpriteData GetSprite(Dictionary<String, SpriteData> sprites, String name)
        {
            return sprites[GetFullName(name)];
        }

        internal static String GetFullName(String textureName)
        {
            return $@"Sprites/Interface/{textureName}";
        }
    }
}
