using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private IMenuBuilder levelSelectBuilder;
        private IMenuBuilder musicTestBuilder;
        private IMenuBuilder settingsMenuBuilder;
        private IMenuBuilder saveProfilesMenuBuilder;

        internal MenuBuilder(EesGame game, IMenuBuilder levelSelectBuilder, IMenuBuilder musicTestBuilder, 
            IMenuBuilder settingsMenuBuilder, IMenuBuilder saveProfilesMenuBuilder)
        {
            this.game = game;
            this.levelSelectBuilder = levelSelectBuilder;
            this.musicTestBuilder = musicTestBuilder;
            this.settingsMenuBuilder = settingsMenuBuilder;
            this.saveProfilesMenuBuilder = saveProfilesMenuBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var items = new MenuItemButton[]
            {
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Unpause"))),
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Continue"))),
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/NewGame"))),
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect")),
                    levelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/SaveProfiles")),
                    saveProfilesMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Settings/Submenu")),
                    settingsMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/MusicTest")),
                    musicTestBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemButton(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/Exit")))
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();

            items[^1].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            return new MenuItemsContainer(items);
        }
    }
}
