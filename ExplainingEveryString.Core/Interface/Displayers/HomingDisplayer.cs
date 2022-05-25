using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class HomingDisplayer : IWeaponDisplayer
    {
        private SpriteData homingAmmo;
        private InterfaceSpriteDisplayer displayer;

        private const Int32 pixelsFromRight = 32 + Constants.MinimapSize;
        private const Int32 pixelsFromBottom = 32;

        public HomingDisplayer(InterfaceSpriteDisplayer displayer)
        {
            this.displayer = displayer;
        }

        public void Draw(PlayerWeaponInterfaceInfo playerWeapon)
        {
            foreach (var index in Enumerable.Range(0, playerWeapon.CurrentAmmo))
            {
                var x = displayer.ScreenWidth - pixelsFromRight - (index + 1) * homingAmmo.Width;
                var y = displayer.ScreenHeight - pixelsFromBottom - homingAmmo.Height;
                var position = new Vector2(x, y);
                displayer.Draw(homingAmmo, position);
            }
        }

        public String[] GetSpritesNames() => new[] { @"Weaponry/Homing" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            homingAmmo = TextureLoadingHelper.GetSprite(sprites, @"Weaponry/Homing");
        }
    }
}
