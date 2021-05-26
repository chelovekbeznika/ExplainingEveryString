using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class SettingsMenuBuilder : IMenuBuilder
    {
        private EesGame game;

        internal SettingsMenuBuilder(EesGame game)
        {
            this.game = game;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            return new MenuItemsContainer(
                new MenuItem[]
                {
                    new MenuItemMusicVolumeSetting(
                        empty: content.Load<Texture2D>(@"Sprites/Menu/Settings/EmptySoundBar"),
                        full: content.Load<Texture2D>(@"Sprites/Menu/Settings/FullSoundBar"),
                        maxBars: 10)
                });
        }
    }
}
