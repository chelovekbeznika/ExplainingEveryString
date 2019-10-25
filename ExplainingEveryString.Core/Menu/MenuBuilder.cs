using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private IMenuBuilder levelSelectBuilder;

        internal MenuBuilder(EesGame game, IMenuBuilder levelSelectBuilder)
        {
            this.game = game;
            this.levelSelectBuilder = levelSelectBuilder;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            ContentManager content = game.Content;
            MenuItem[] items = new MenuItem[]
            {
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Unpause")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                    levelSelectBuilder.BuildMenu(menuVisiblePart), menuVisiblePart),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[4].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            return new MenuItemsContainer(items);
        }
    }
}
