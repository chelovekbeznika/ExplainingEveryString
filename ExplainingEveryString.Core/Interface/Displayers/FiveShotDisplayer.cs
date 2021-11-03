using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class FiveShotDisplayer : IWeaponDisplayer
    {
        private const Int32 pixelsFromRight = 16;
        private const Int32 pixelsFromBottom = 16;
        private const Int32 heightAmplitudePixels = 1;

        private SpriteData shell;
        private SpriteData emptyShell;
        private InterfaceSpriteDisplayer displayer;

        internal FiveShotDisplayer(InterfaceSpriteDisplayer displayer)
        {
            this.displayer = displayer;
        }

        public void Draw(PlayerWeaponInterfaceInfo playerWeapon)
        {
            foreach (var index in Enumerable.Range(0, playerWeapon.MaxAmmo))
            {
                var x = displayer.ScreenWidth - pixelsFromRight - (index + 1) * shell.Width;
                var y = displayer.ScreenHeight - pixelsFromBottom - shell.Height 
                    + (index % 3 == 0 
                        ? 0 
                        : index % 3 == 1 
                            ? -heightAmplitudePixels 
                            : heightAmplitudePixels);
                var position = new Vector2(x, y);
                var texture = index < playerWeapon.CurrentAmmo ? shell : emptyShell;
                displayer.Draw(texture, position);
            }
        }

        public String[] GetSpritesNames() => new[] { @"Weaponry\FiveShot", @"Weaponry\FiveShotEmpty" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            shell = TextureLoadingHelper.GetSprite(sprites, @"Weaponry\FiveShot");
            emptyShell = TextureLoadingHelper.GetSprite(sprites, @"Weaponry\FiveShotEmpty");
        }
    }
}
