using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class LevelSelectMenuBuilder : IMenuBuilder
    {
        private EesGame game;
        private ContentManager Content => game.Content;
        private LevelSequnceSpecification levelSequenceSpecification;

        internal LevelSelectMenuBuilder(EesGame game, LevelSequnceSpecification levelSequenceSpecification)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            List<MenuItem> items = new List<MenuItem>();
            foreach (LevelsBlockSpecification levelsBlock in levelSequenceSpecification.LevelsBlocks)
            {
                MenuItem item = new MenuItemWithContainer(Content.Load<Texture2D>(levelsBlock.ButtonSprite),
                    GetMenuContainerForBlock(levelsBlock), menuVisiblePart);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }

        private MenuItemsContainer GetMenuContainerForBlock(LevelsBlockSpecification levelsBlock)
        {
            List<MenuItem> items = new List<MenuItem>();
            IEnumerable<LevelSpecification> levels = levelSequenceSpecification.Levels
                .Where(l => l.LevelsBlockId == levelsBlock.Id);
            foreach (LevelSpecification level in levels)
            {
                MenuItem item = new MenuItem(Content.Load<Texture2D>(level.ButtonSprite));
                item.ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueFrom(level.LevelData);
                item.IsVisible = () => game.GameState.LevelAvailable(level.LevelData);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }
    }
}
