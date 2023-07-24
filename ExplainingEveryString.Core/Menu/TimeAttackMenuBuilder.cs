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

        internal TimeAttackMenuBuilder(EesGame game, IMenuBuilder levelSelectMenuBuilder)
        {
            this.levelSelectMenuBuilder = levelSelectMenuBuilder;
            this.content = game.Content;
        }

        public MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart)
        {
            return new MenuItemsContainer(new MenuItem[]
            {
                new MenuItemWithContainer(new OneSpriteDisplayer(content.Load<Texture2D>(@"Sprites/Menu/OneLevelTimeAttack")),
                    levelSelectMenuBuilder.BuildMenu(menuVisiblePart), menuVisiblePart)
            });
        }
    }
}
