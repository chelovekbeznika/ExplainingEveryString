using ExplainingEveryString.Core.Menu.Settings;
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
        private EventHandler<EventArgs> saveSettingsHandler;

        internal SettingsMenuBuilder(EesGame game, EventHandler<EventArgs> saveSettingsHandler)
        {
            this.game = game;
            this.saveSettingsHandler = saveSettingsHandler;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var saveSettings = new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/Settings/Save"));
            var container = new MenuItemsContainer(
                new MenuItem[]
                {
                    new MenuItemVolumeSetting(
                        empty: content.Load<Texture2D>(@"Sprites/Menu/Settings/EmptySoundBar"),
                        full: content.Load<Texture2D>(@"Sprites/Menu/Settings/FullMusicBar"),
                        maxBars: CurrentSettings.MaxSoundBars,
                        getItem: () => SettingsAccess.GetCurrentSettings().MusicVolume,
                        setItem: volume => SettingsAccess.GetCurrentSettings().MusicVolume = volume),
                    new MenuItemVolumeSetting(
                        empty: content.Load<Texture2D>(@"Sprites/Menu/Settings/EmptySoundBar"),
                        full: content.Load<Texture2D>(@"Sprites/Menu/Settings/FullSoundBar"),
                        maxBars: CurrentSettings.MaxSoundBars,
                        getItem: () => SettingsAccess.GetCurrentSettings().SoundVolume,
                        setItem: volume => SettingsAccess.GetCurrentSettings().SoundVolume = volume),
                    saveSettings
                });
            saveSettings.ItemCommandExecuteRequested += saveSettingsHandler;
            return container;
        }
    }
}
