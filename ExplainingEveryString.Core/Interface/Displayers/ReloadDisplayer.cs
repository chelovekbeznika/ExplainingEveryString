using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private readonly Func<Boolean> isCursorVisible;
        private SpriteData magazineBackground;
        private SpriteData magazine;
        private SpriteData ammo;
        private SpriteData cursorBase;
        private SpriteData cursorIndicator;

        public ReloadDisplayer(InterfaceDrawController interfaceSpritesDisplayer, Func<bool> isCursorVisible)
        {
            this.displayer = interfaceSpritesDisplayer;
            this.isCursorVisible = isCursorVisible;
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

            if (isCursorVisible())
            {
                var mouseCoordinates = ScreenCoordinatesHelper.ConvertToInnerScreenCoordinates(Mouse.GetState().X, Mouse.GetState().Y);
                var basePosition = mouseCoordinates - new Vector2(cursorBase.Width / 2, cursorBase.Height / 2);
                var indicatorOffset = new Vector2(cursorBase.Width * reloadRemained - cursorBase.Width / 2, 0);
                var indicatorPosition = mouseCoordinates + indicatorOffset 
                    - new Vector2(cursorIndicator.Width / 2, cursorIndicator.Height / 2);
                displayer.Draw(cursorBase, basePosition);
                displayer.Draw(cursorIndicator, indicatorPosition);
            }
        }
    }
}
