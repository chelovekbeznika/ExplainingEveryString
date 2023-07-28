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
        private readonly Boolean registerForRecordShow = false;

        internal static LevelSelectMenuBuilder ContinueStory(EesGame game, LevelSequenceSpecification levelSequenceSpecification) =>
            new LevelSelectMenuBuilder(game, levelSequenceSpecification, (levelName) => game.GameState.ContinueFrom(levelName), false);

        internal static LevelSelectMenuBuilder OneLevelRun(EesGame game, LevelSequenceSpecification levelSequenceSpecification) =>
            new LevelSelectMenuBuilder(game, levelSequenceSpecification, (levelName) => game.GameState.StartOneLevelRun(levelName), true);

        private LevelSelectMenuBuilder(EesGame game, LevelSequenceSpecification levelSequenceSpecification, 
            Action<String> commandHanler, Boolean registerForRecordShow)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.levelStart = commandHanler;
            this.registerForRecordShow = registerForRecordShow;
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
            var levels = levelSequenceSpecification.Levels
                .Where(l => l.LevelsBlockId == levelsBlock.Id);
            var items = levels.Select(level =>
            {
                var displayer = new OneSpriteDisplayer(Content.Load<Texture2D>(level.ButtonSprite));
                var item = new MenuItemButton(displayer);
                var levelName = level.LevelData;
                item.ItemCommandExecuteRequested += (sender, e) => levelStart(levelName);
                item.IsVisible = () => game.GameState.LevelAvailable(levelName);
                if (registerForRecordShow)
                    game.GameState.GameTimeState.RegisterLevelTimeButton(levelName, item);
                return item;
            });
            return new MenuItemsContainer(items.ToArray());
        }
    }
}
