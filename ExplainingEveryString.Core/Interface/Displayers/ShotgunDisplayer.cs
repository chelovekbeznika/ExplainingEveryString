using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class ShotgunDisplayer : IWeaponDisplayer
    {
        private const Int32 betweenShellsPixels = 4;
        private const Int32 pixelsFromRight = 12;
        private const Int32 pixelsFromBottom = 16;

        private SpriteData shell;
        private SpriteData emptyShell;
        private InterfaceSpriteDisplayer displayer;

        internal ShotgunDisplayer(InterfaceSpriteDisplayer displayer)
        {
            this.displayer = displayer;
        }

        public void Draw(PlayerWeaponInterfaceInfo playerWeapon)
        {
            foreach (var index in Enumerable.Range(0, playerWeapon.MaxAmmo))
            {
                var x = displayer.ScreenWidth - pixelsFromRight - (index + 1) * (betweenShellsPixels + shell.Width);
                var y = displayer.ScreenHeight - pixelsFromBottom - shell.Height;
                var position = new Vector2(x, y);
                var texture = index < playerWeapon.CurrentAmmo ? shell : emptyShell;
                displayer.Draw(texture, position);
            }
        }

        public String[] GetSpritesNames() => new[] { @"Weaponry\Shotgun", @"Weaponry\ShotgunEmpty" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            shell = TextureLoadingHelper.GetSprite(sprites, @"Weaponry\Shotgun");
            emptyShell = TextureLoadingHelper.GetSprite(sprites, @"Weaponry\ShotgunEmpty");
        }
    }
}
