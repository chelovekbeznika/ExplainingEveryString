using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemWithContainer : MenuItem
    {
        private MenuItemsContainer container;
        private MenuVisiblePart menuVisiblePart;

        internal override MenuItemsContainer ParentContainer
        {
            get => base.ParentContainer;
            set
            {
                base.ParentContainer = value;
                container.ParentContainer = value;
            }
        }

        internal MenuItemWithContainer(Texture2D sprite, MenuItemsContainer container, MenuVisiblePart menuVisiblePart)
            : base(sprite)
        {
            this.container = container;
            this.menuVisiblePart = menuVisiblePart;
        }

        internal void SetParentContainer(MenuItemsContainer parentContainer)
        {
            container.ParentContainer = parentContainer;
        }

        internal override void RequestCommandExecution()
        {
            menuVisiblePart.CurrentButtonsContainer = container;
            base.RequestCommandExecution();
        }
    }
}
