using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework.Graphics;
using System;

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
            var saveSettings = new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Settings/Save")));
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
                    new MenuItemControlDeviceSetting(
                        gamePad: content.Load<Texture2D>(@"Sprites/Menu/Settings/GamePad"),
                        keyboard: content.Load<Texture2D>(@"Sprites/Menu/Settings/Keyboard")),
                    new MenuItemResolutionSetting(
                        fontsStorage: game.FontsStorage,
                        adapter: game.GraphicsDevice.Adapter),
                    new MenuItemFullscreenSetting(
                        window: content.Load<Texture2D>(@"Sprites/Menu/Settings/Window"),
                        fullscreen: content.Load<Texture2D>(@"Sprites/Menu/Settings/Fullscreen")),
                    saveSettings
                });
            saveSettings.ItemCommandExecuteRequested += saveSettingsHandler;
            container.ContainerAppearedOnScreen += 
                (sender, e) => SettingsAccess.InitSettingsFromConfiguration(ConfigurationAccess.GetCurrentConfig());
            return container;
        }
    }
}
