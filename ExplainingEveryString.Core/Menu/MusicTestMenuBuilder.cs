using ExplainingEveryString.Data.Menu;
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
    internal class MusicTestMenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private ContentManager Content => game.Content;
        private MusicTestButtonSpecification[] buttonsSpecification;

        internal MusicTestMenuBuilder(EesGame game, MusicTestButtonSpecification[] buttonsSpecification)
        {
            this.game = game;
            this.buttonsSpecification = buttonsSpecification;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var stopMusicButton = new MenuItem(Content.Load<Texture2D>(@"Sprites/Menu/StopMusic"));
            stopMusicButton.ItemCommandExecuteRequested += (sender, e) => game.GameState.StopMusicInMenu();
            return new MenuItemsContainer(new MenuItem[] { stopMusicButton }.Concat(buttonsSpecification.Select(buttonSpecification =>
            {
                var button = new MenuItem(Content.Load<Texture2D>(buttonSpecification.Sprite));
                button.ItemCommandExecuteRequested += (sender, e) => game.GameState.SongInMenuSelected(buttonSpecification.SongName);
                return button;
            })).ToArray());
        }
    }
}
