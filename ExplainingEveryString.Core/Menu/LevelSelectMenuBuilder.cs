using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class LevelSelectMenuBuilder : IMenuBuilder
    {
        private ContentManager Content => game.Content;
        private readonly EesGame game;
        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private readonly Action<String> levelStart;

        internal static LevelSelectMenuBuilder ContinueStory(EesGame game, LevelSequenceSpecification levelSequenceSpecification) =>
            new LevelSelectMenuBuilder(game, levelSequenceSpecification, (levelName) => game.GameState.ContinueFrom(levelName));

        internal static LevelSelectMenuBuilder OneLevelRun(EesGame game, LevelSequenceSpecification levelSequenceSpecification) =>
            new LevelSelectMenuBuilder(game, levelSequenceSpecification, (levelName) => game.GameState.StartOneLevelRun(levelName));

        private LevelSelectMenuBuilder(EesGame game, LevelSequenceSpecification levelSequenceSpecification, Action<String> commandHanler)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.levelStart = commandHanler;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var items = new List<MenuItemButton>();
            foreach (var levelsBlock in levelSequenceSpecification.LevelsBlocks)
            {
                var item = new MenuItemWithContainer(new OneSpriteDisplayer(Content.Load<Texture2D>(levelsBlock.ButtonSprite)),
                    GetMenuContainerForBlock(levelsBlock), menuVisiblePart);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }

        private MenuItemsContainer GetMenuContainerForBlock(LevelsBlockSpecification levelsBlock)
        {
            var items = new List<MenuItemButton>();
            var levels = levelSequenceSpecification.Levels
                .Where(l => l.LevelsBlockId == levelsBlock.Id);
            foreach (var level in levels)
            {
                var displayer = new OneSpriteDisplayer(Content.Load<Texture2D>(level.ButtonSprite));
                var item = new MenuItemButton(displayer);
                item.ItemCommandExecuteRequested += (sender, e) => levelStart(level.LevelData);
                item.IsVisible = () => game.GameState.LevelAvailable(level.LevelData);
                items.Add(item);
            }
            return new MenuItemsContainer(items.ToArray());
        }
    }
}
