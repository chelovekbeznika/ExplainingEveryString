using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class ReloadDisplayer : IDisplayer
    {
        private const String MagazineBackgroundTexture = "MagazineTopPart";
        private const String MagazineTexture = "Magazine";
        private const String AmmoTexture = "AmmoClip";
        private const Int32 pixelsFromRight = 8 + Constants.MinimapSize;
        private const Int32 pixelsFromBottom = 40;
        private const Int32 ammoDroppingWayLength = 128;

        private InterfaceSpriteDisplayer displayer;
        private SpriteData magazineBackground;
        private SpriteData magazine;
        private SpriteData ammo;

        public ReloadDisplayer(InterfaceSpriteDisplayer interfaceSpritesDisplayer)
        {
            this.displayer = interfaceSpritesDisplayer;
        }

        public String[] GetSpritesNames() => new[] { MagazineBackgroundTexture, MagazineTexture, AmmoTexture };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            magazineBackground = sprites[TextureLoadingHelper.GetFullName(MagazineBackgroundTexture)];
            magazine = sprites[TextureLoadingHelper.GetFullName(MagazineTexture)];
            ammo = sprites[TextureLoadingHelper.GetFullName(AmmoTexture)];
        }

        public void Draw(Single reloadRemained)
        {
            var magazinePosition = new Vector2(
                x: displayer.ScreenWidth - pixelsFromRight - magazine.Width, 
                y: displayer.ScreenHeight - pixelsFromBottom - magazine.Height);
            var ammoPosition = new Vector2(
                x: magazinePosition.X, y: magazinePosition.Y - reloadRemained * ammoDroppingWayLength);

            displayer.Draw(magazineBackground, magazinePosition);
            displayer.Draw(ammo, ammoPosition);
            displayer.Draw(magazine, magazinePosition);
        }
    }
}
