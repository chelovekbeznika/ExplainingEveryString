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

        internal MenuItem[] BuildMenu()
        {
            ContentManager content = game.Content;
            MenuItem[] items = new MenuItem[]
            {
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };
            items[0].ItemCommandExecuteRequested += (sender, e) => game.GameState.StartNewGame();
            items[1].ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueCurrentGame();
            items[3].ItemCommandExecuteRequested += (sender, e) => game.Exit();
            return items;
        }
    }
}
