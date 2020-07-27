using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal interface IDisplayer
    {
        String[] GetSpritesNames();
        void InitSprites(Dictionary<String, SpriteData> sprites);
    }

    internal interface IWeaponDisplayer : IDisplayer
    {
        void Draw(PlayerWeaponInterfaceInfo playerWeapon);
    }
}
