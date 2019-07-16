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
        internal MenuItem[] BuildMenu(ContentManager content)
        {
            return new MenuItem[]
            {
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/NewGame")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Continue")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/LevelSelect")),
                new MenuItem(content.Load<Texture2D>(@"Sprites/Menu/Exit"))
            };
        }
    }
}
