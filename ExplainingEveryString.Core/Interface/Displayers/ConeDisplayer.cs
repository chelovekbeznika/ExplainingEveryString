using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class ConeDisplayer : IWeaponDisplayer
    {
        private const String FullTexture = @"Weaponry/Cone";
        private const String EmptyTexture = @"Weaponry/ConeEmpty";
        private const Int32 pixelsFromRight = 16 + Constants.MinimapSize;
        private const Int32 pixelsFromBottom = 16;

        private readonly InterfaceSpriteDisplayer displayer;
        private SpriteData full;
        private SpriteData empty;

        internal ConeDisplayer(InterfaceSpriteDisplayer displayer)
        {
            this.displayer = displayer;
        }

        public void Draw(PlayerWeaponInterfaceInfo playerWeapon)
        {
            var remained = (Single)playerWeapon.CurrentAmmo/ playerWeapon.MaxAmmo;
            var position = new Vector2(
                x: displayer.ScreenWidth - pixelsFromRight - full.Width, 
                y: displayer.ScreenHeight - pixelsFromBottom - full.Height);
            displayer.Draw(full, position, new LeftPartDisplayer(), remained);
            displayer.Draw(empty, position, new RightPartDisplayer(), 1 - remained);
        }

        public String[] GetSpritesNames() => new[] { FullTexture, EmptyTexture };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            full = TextureLoadingHelper.GetSprite(sprites, FullTexture);
            empty = TextureLoadingHelper.GetSprite(sprites, EmptyTexture);
        }
    }
}
