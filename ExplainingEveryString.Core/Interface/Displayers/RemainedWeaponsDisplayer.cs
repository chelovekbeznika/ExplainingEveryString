﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class RemainedWeaponsDisplayer : IDisplayer
    {
        private const Int32 betweenIcons = 8;
        private const Int32 pixelsFromRight = 32;
        private const Int32 pixelsFromTop = 32;
        private readonly IReadOnlyList<String> ExistingWeapons = new[] { "Default", "Shotgun", "RocketLauncher", "Cone", "Homing" };
        private const string Border = @"WeaponIcons/Border";

        private Dictionary<String, SpriteData> iconsSprites;
        private SpriteData border;
        private InterfaceSpriteDisplayer spriteDisplayer;

        public String[] GetSpritesNames() => ExistingWeapons.Select(name => $@"WeaponIcons/{name}").Concat(new[] { Border }).ToArray();

        internal RemainedWeaponsDisplayer(InterfaceSpriteDisplayer spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            iconsSprites = ExistingWeapons.ToDictionary(name => name, name => TextureLoadingHelper.GetSprite(sprites, $@"WeaponIcons/{name}"));
            border = TextureLoadingHelper.GetSprite(sprites, Border);
        }

        internal void Draw(PlayerWeaponInterfaceInfo playerWeapons)
        {
            var weapons = playerWeapons.AvailableWeapons;
            
            var iconPlace = 0;
            var iconWidth = iconsSprites[GameModel.Constants.DefaultPlayerWeapon].Width;
            var iconHeight = iconsSprites[GameModel.Constants.DefaultPlayerWeapon].Height;
            var placeFirstIconAt = spriteDisplayer.ScreenWidth - pixelsFromRight - (iconWidth + betweenIcons) * weapons.Count + betweenIcons;
            foreach (var weaponName in weapons)
            {
                var iconPosition = new Vector2(x: placeFirstIconAt + (iconWidth + betweenIcons) * iconPlace, y: pixelsFromTop);
                var weaponIcon = iconsSprites[weaponName];
                spriteDisplayer.Draw(weaponIcon, iconPosition);

                if (weapons.FindIndex(weapon => weapon == playerWeapons.SelectedWeapon) == iconPlace)
                {
                    var widthDiff = (border.Width - iconWidth) / 2;
                    var heightDiff = (border.Height - iconHeight) / 2;
                    var borderPosition = new Vector2(x: iconPosition.X - widthDiff, y: iconPosition.Y - heightDiff);
                    spriteDisplayer.Draw(border, borderPosition);
                }
                iconPlace += 1;
            }
        }
    }
}
