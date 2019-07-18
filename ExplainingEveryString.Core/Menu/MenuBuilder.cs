using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuBuilder
    {
        private Game game;

        internal MenuBuilder(Game game)
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
            items[3].ItemCommandExecuteRequested += (sender, e) => game.Exit();
            return items;
        }
    }
}
