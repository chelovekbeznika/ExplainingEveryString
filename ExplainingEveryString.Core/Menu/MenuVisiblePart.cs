using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuVisiblePart
    {
        private MenuItemPositionsMapper positionsMapper;
        private MenuItemDisplayer itemDisplayer;

        internal MenuItemsContainer CurrentButtonsContainer { get; set; }

        internal MenuVisiblePart(MenuBuilder builder, MenuItemPositionsMapper positionsMapper, MenuItemDisplayer itemDisplayer)
        {
            this.CurrentButtonsContainer = builder.BuildMenu(this);
            this.positionsMapper = positionsMapper;
            this.itemDisplayer = itemDisplayer;
        }

        internal void Draw()
        {
            IEnumerable<MenuItem> items = CurrentButtonsContainer.Items.Where(item => item.IsVisible());
            Point[] positions = positionsMapper.GetItemsPositions(items.Select(item => item.GetSize()).ToArray());
            foreach (var pair in items.Zip(positions, (Item, Position) => new { Item, Position }))
            {
                itemDisplayer.Draw(pair.Item, pair.Position);
            }
        }

        internal void TryToGetBack()
        {
            MenuItemsContainer parentContainer = CurrentButtonsContainer.ParentContainer;
            if (parentContainer != null)
                CurrentButtonsContainer = parentContainer;
        }
    }
}
