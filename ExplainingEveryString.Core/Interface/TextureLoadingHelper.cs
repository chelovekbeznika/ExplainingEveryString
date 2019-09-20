using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal static class TexturesHelper
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
