using ExplainingEveryString.Core.Assets;
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
        private const String CursorBase = "ReloadCursorBase";
        private const String CursorIndicator = "ReloadCursorIndicator";
        private const Int32 pixelsFromRight = 8 + Displaying.Constants.MinimapSize;
        private const Int32 pixelsFromBottom = 40;
        private const Int32 ammoDroppingWayLength = 128;

        private readonly InterfaceDrawController displayer;
        private SpriteData magazineBackground;
        private SpriteData magazine;
        private SpriteData ammo;
        private SpriteData cursorBase;
        private SpriteData cursorIndicator;

        public ReloadDisplayer(InterfaceDrawController interfaceSpritesDisplayer)
        {
            this.displayer = interfaceSpritesDisplayer;
        }

        public String[] GetSpritesNames() => new[] 
        { 
            MagazineBackgroundTexture, MagazineTexture, 
            AmmoTexture, CursorBase, CursorIndicator
        };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            magazineBackground = sprites[TextureLoadingHelper.GetFullName(MagazineBackgroundTexture)];
            magazine = sprites[TextureLoadingHelper.GetFullName(MagazineTexture)];
            ammo = sprites[TextureLoadingHelper.GetFullName(AmmoTexture)];
            cursorBase = sprites[TextureLoadingHelper.GetFullName(CursorBase)];
            cursorIndicator = sprites[TextureLoadingHelper.GetFullName(CursorIndicator)];
        }

        public void Draw(PlayerInterfaceInfo player)
        {
            var reloadRemained = player.Weapon.ReloadRemained.Value;
            var cursorPosition = player.CursorPosition;

            DrawCornerIndicator(reloadRemained);
            DrawCursorIndicator(cursorPosition, reloadRemained);
        }

        private void DrawCornerIndicator(Single reloadRemained)
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

        private void DrawCursorIndicator(Vector2 cursorPosition, Single reloadRemained)
        {
            var basePosition = cursorPosition - new Vector2(cursorBase.Width / 2, cursorBase.Height / 2);
            var indicatorOffset = new Vector2(cursorBase.Width * reloadRemained - cursorBase.Width / 2, 0);
            var indicatorPosition = cursorPosition + indicatorOffset
                - new Vector2(cursorIndicator.Width / 2, cursorIndicator.Height / 2);
            displayer.Draw(cursorBase, basePosition);
            displayer.Draw(cursorIndicator, indicatorPosition);
        }
    }
}
