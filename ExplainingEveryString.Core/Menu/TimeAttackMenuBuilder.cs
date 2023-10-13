using ExplainingEveryString.Core.GameState;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu
{
    internal class TimeAttackMenuBuilder : IMenuBuilder
    {
        private readonly IMenuBuilder levelSelectMenuBuilder;
        private readonly ContentManager content;
        private readonly GameStateManager gameState;

        internal TimeAttackMenuBuilder(EesGame game, IMenuBuilder levelSelectMenuBuilder)
        {
            this.levelSelectMenuBuilder = levelSelectMenuBuilder;
            this.content = game.Content;
            this.gameState = game.GameState;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            var oneLevelDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/OneLevelTimeAttack"));
            var levelButtonsContainer = levelSelectMenuBuilder.BuildMenu(menuVisiblePart);
            var oneLevelButton = new MenuItemWithContainer(oneLevelDisplayer, levelButtonsContainer, menuVisiblePart)
            {
                Text = "ONE LEVEL"
            };

            var wholeGameDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/WholeGameTimeAttack"));
            var wholeGameButton = new MenuItemButton(wholeGameDisplayer);
            wholeGameButton.ItemCommandExecuteRequested += (sender, args) => gameState.StartWholeGameRun();

            var recordsTableDisplayer = new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/RecordsTable"));
            var recordsTableButton = new MenuItemButton(recordsTableDisplayer)
            { 
                Text = "RECORDS" 
            };
            recordsTableButton.ItemCommandExecuteRequested += (sender, args) => gameState.ShowTimeTableFromMenu();

            gameState.GameTimeState.RegisterWholeGameTimeButton(wholeGameButton);
            return new MenuItemsContainer(new MenuItem[] { oneLevelButton, wholeGameButton, recordsTableButton });
        }
    }
}
