using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private IMenuBuilder levelSelectBuilder;
        private IMenuBuilder musicTestBuilder;
        private IMenuBuilder settingsMenuBuilder;

        internal MenuBuilder(EesGame game, IMenuBuilder levelSelectBuilder, IMenuBuilder musicTestBuilder, IMenuBuilder settingsMenuBuilder)
        {
            this.game = game;
            this.levelSelectBuilder = levelSelectBuilder;
            this.musicTestBuilder = musicTestBuilder;
            this.settingsMenuBuilder = settingsMenuBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var items = new MenuItemButton[]
            {
                new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/Unpause")),
                new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                    levelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/MusicTest"),
                    musicTestBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/Settings/Submenu"),
                    settingsMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemButton(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();
            
            items[6].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            return new MenuItemsContainer(items);
        }
    }
}
