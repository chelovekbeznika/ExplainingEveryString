﻿using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class SettingsMenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private CurrentSettings settings;

        internal SettingsMenuBuilder(EesGame game, CurrentSettings settings)
        {
            this.game = game;
            this.settings = settings;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var container = new MenuItemsContainer(
                new MenuItem[]
                {
                    new MenuItemMusicVolumeSetting(
                        settings: settings,
                        empty: content.Load<Texture2D>(@"Sprites/Menu/Settings/EmptySoundBar"),
                        full: content.Load<Texture2D>(@"Sprites/Menu/Settings/FullSoundBar"),
                        maxBars: CurrentSettings.MaxSoundBars),
                    new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/Settings/Save"))
                });
            (container.Items[container.Items.Length - 1] as MenuItemButton).ItemCommandExecuteRequested += (sender, e) =>
            {
                var config = ConfigurationAccess.GetCurrentConfig();
                SettingsConfigurationConverter.SettingsIntoConfiguration(config, settings);
                ConfigurationAccess.SaveCurrentConfig();
                game.GameState.ConfigChanged();
            };
            return container;
        }
    }
}
