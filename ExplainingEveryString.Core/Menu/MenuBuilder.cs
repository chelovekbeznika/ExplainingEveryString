using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder
    {
        private EesGame game;

        internal MenuBuilder(EesGame game)
        {
            this.game = game;
        }

        internal MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            ContentManager content = game.Content;
            MenuItem[] items = new MenuItem[]
            {
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Unpause")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect"),
                    GetSelectLevelMenu(content, menuVisiblePart), menuVisiblePart),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };

            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.TryPauseSwitch();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();
            items[2].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[4].ItemCommandExecuteRequested += (sender, e) => game.Exit();

            items[0].IsVisible = () => game.GameState.IsPaused;
            return new MenuItemsContainer(items);
        }

        private MenuItemsContainer GetSelectLevelMenu(ContentManager content, MenuVisiblePart menuVisiblePart)
        {
            MenuItemsContainer childContainer = new MenuItemsContainer(
                new MenuItem[]
                {
                    new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect/11")),
                    new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect/12")),
                    new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect/13")),
                    new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect/14")),
                });
            MenuItemsContainer levelSelectContainer = new MenuItemsContainer(
                new MenuItem[]
                {
                    new MenuItemWithContainer(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect/1X"),
                        childContainer, menuVisiblePart)
                });
            return levelSelectContainer;
        }
    }
}
