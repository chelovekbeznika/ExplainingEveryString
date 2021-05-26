using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemWithContainer : MenuItemButton
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
            this.IsVisible = () => container.Items.Any(item => item.IsVisible());
        }

        internal void SetParentContainer(MenuItemsContainer parentContainer)
        {
            container.ParentContainer = parentContainer;
        }

        internal override void RequestCommandExecution()
        {
            menuVisiblePart.CurrentButtonsContainer = container;
        }
    }
}
