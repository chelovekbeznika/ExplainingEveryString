using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class RemainedWeaponsDisplayer : IDisplayer
    {
        private const Int32 betweenIcons = 8;
        private const Int32 pixelsFromRight = 32;
        private const Int32 pixelsFromTop = 32;
        private const string Border = @"WeaponIcons/Border";

        private Dictionary<String, SpriteData> iconsSprites;
        private SpriteData border;
        private InterfaceSpriteDisplayer spriteDisplayer;

        public String[] GetSpritesNames() => WeaponNames.AllExisting
            .Select(name => $@"WeaponIcons/{name}").Concat(new[] { Border }).ToArray();

        internal RemainedWeaponsDisplayer(InterfaceSpriteDisplayer spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            iconsSprites = WeaponNames.AllExisting
                .ToDictionary(name => name, name => TextureLoadingHelper.GetSprite(sprites, $@"WeaponIcons/{name}"));
            border = TextureLoadingHelper.GetSprite(sprites, Border);
        }

        internal void Draw(PlayerWeaponInterfaceInfo playerWeapons)
        {
            var weapons = playerWeapons.AvailableWeapons;
            
            var iconWidth = iconsSprites[GameModel.WeaponNames.Default].Width;
            var iconHeight = iconsSprites[GameModel.WeaponNames.Default].Height;
            var placeFirstIconAt = spriteDisplayer.ScreenWidth - pixelsFromRight - iconWidth;
            foreach (var (iconPlace, weaponName) in weapons.Select((weapon, index) => (index, weapon)))
            {
                var iconPosition = new Vector2(x: placeFirstIconAt, y: pixelsFromTop + (iconHeight + betweenIcons) * iconPlace);
                var weaponIcon = iconsSprites[weaponName];
                spriteDisplayer.Draw(weaponIcon, iconPosition);

                if (weapons.FindIndex(weapon => weapon == playerWeapons.SelectedWeapon) == iconPlace)
                {
                    var widthDiff = (border.Width - iconWidth) / 2;
                    var heightDiff = (border.Height - iconHeight) / 2;
                    var borderPosition = new Vector2(x: iconPosition.X - widthDiff, y: iconPosition.Y - heightDiff);
                    spriteDisplayer.Draw(border, borderPosition);
                }
            }
        }
    }
}
