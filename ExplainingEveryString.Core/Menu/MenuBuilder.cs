using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private IMenuBuilder levelSelectBuilder;
        private IMenuBuilder musicTestBuilder;

        internal MenuBuilder(EesGame game, IMenuBuilder levelSelectBuilder, IMenuBuilder musicTestBuilder)
        {
            this.game = game;
            this.levelSelectBuilder = levelSelectBuilder;
            this.musicTestBuilder = musicTestBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var content = game.Content;
            var items = new MenuItem[]
            {
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Unpause")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                    levelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/MusicTest"),
                    musicTestBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[5].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            return new MenuItemsContainer(items);
        }
    }
}
