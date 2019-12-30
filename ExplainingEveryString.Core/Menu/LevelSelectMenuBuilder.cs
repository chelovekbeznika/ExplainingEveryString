using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

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
            var items = new List<MenuItem>();
            foreach (var levelsBlock in levelSequenceSpecification.LevelsBlocks)
            {
                var item = new MenuItemWithContainer(Content.Load<Texture2D>(levelsBlock.ButtonSprite),
                    GetMenuContainerForBlock(levelsBlock), menuVisiblePart);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }

        private MenuItemsContainer GetMenuContainerForBlock(LevelsBlockSpecification levelsBlock)
        {
            var items = new List<MenuItem>();
            var levels = levelSequenceSpecification.Levels
                .Where(l => l.LevelsBlockId == levelsBlock.Id);
            foreach (var level in levels)
            {
                var item = new MenuItem(Content.Load<Texture2D>(level.ButtonSprite));
                item.ItemCommandExecuteRequested += (sender, e) => game.GameState.ContinueFrom(level.LevelData);
                item.IsVisible = () => game.GameState.LevelAvailable(level.LevelData);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }
    }
}
