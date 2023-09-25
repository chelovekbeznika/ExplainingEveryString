using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

        private CustomFont numbersFont;
        private Dictionary<String, SpriteData> iconsSprites;
        private Dictionary<String, Int32> weaponNumbers;
        private SpriteData border;
        private InterfaceSpriteDisplayer spriteDisplayer;

        public String[] GetSpritesNames() => WeaponNames.AllExisting
            .Select(name => $@"WeaponIcons/{name}").Concat(new[] { Border }).ToArray();

        internal RemainedWeaponsDisplayer(InterfaceSpriteDisplayer spriteDisplayer, CustomFont numbersFont)
        {
            this.spriteDisplayer = spriteDisplayer;
            this.numbersFont = numbersFont;
            this.weaponNumbers = new Dictionary<String, Int32>(
                WeaponNames.AllExisting
                .Select((name, index) => (name, index))
                .Select(x => new KeyValuePair<String, Int32>(x.name, x.index + 1)));
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
            var usingKeyboard = ConfigurationAccess.GetCurrentConfig().Input.PreferredControlDevice == ControlDevice.Keyboard
                || !GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
            
            var iconWidth = iconsSprites[WeaponNames.Default].Width;
            var iconHeight = iconsSprites[WeaponNames.Default].Height;
            var placeFirstIconAt = spriteDisplayer.ScreenWidth - pixelsFromRight - iconWidth;
            foreach (var (iconPlace, weaponName) in weapons.Select((weapon, index) => (index, weapon)))
            {
                var iconPosition = new Vector2(x: placeFirstIconAt, y: pixelsFromTop + (iconHeight + betweenIcons) * iconPlace);
                var weaponIcon = iconsSprites[weaponName];
                spriteDisplayer.Draw(weaponIcon, iconPosition);

                if (usingKeyboard)
                {
                    var text = weaponNumbers[weaponName].ToString();
                    var textSize = numbersFont.GetSize(text);
                    var textPosition = new Vector2(
                        x: iconPosition.X - iconWidth / 2 - textSize.X / 2,
                        y: iconPosition.Y + iconHeight / 2 - textSize.Y / 2);
                    spriteDisplayer.DrawText(text, textPosition, numbersFont);
                }

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
