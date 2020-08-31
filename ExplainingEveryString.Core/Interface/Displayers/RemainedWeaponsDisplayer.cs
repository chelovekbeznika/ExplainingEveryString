using Microsoft.Xna.Framework;
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
        private readonly IReadOnlyList<String> ExistingWeapons = new[] { "Default", "Shotgun", "RocketLauncher", "Cone" };

        private Dictionary<String, SpriteData> iconsSprites;
        private InterfaceSpriteDisplayer spriteDisplayer;

        public String[] GetSpritesNames() => ExistingWeapons.Select(name => $@"WeaponIcons/{name}").ToArray();

        internal RemainedWeaponsDisplayer(InterfaceSpriteDisplayer spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            iconsSprites = ExistingWeapons.ToDictionary(name => name, name => TextureLoadingHelper.GetSprite(sprites, $@"WeaponIcons/{name}"));
        }

        internal void Draw(PlayerWeaponInterfaceInfo playerWeapons)
        {
            var weapons = playerWeapons.AvailableWeapons;
            var iconPlace = 0;
            var iconWidth = iconsSprites[GameModel.Constants.DefaultPlayerWeapon].Width;
            var placeFirstIconAt = spriteDisplayer.ScreenWidth - pixelsFromRight - (iconWidth + betweenIcons) * weapons.Count + betweenIcons;
            foreach (var weaponName in weapons)
            {
                var iconPosition = new Vector2(x: placeFirstIconAt + (iconWidth + betweenIcons) * iconPlace, y: pixelsFromTop);
                var weaponIcon = iconsSprites[weaponName];
                spriteDisplayer.Draw(weaponIcon, iconPosition);
                iconPlace += 1;
            }
        }
    }
}
