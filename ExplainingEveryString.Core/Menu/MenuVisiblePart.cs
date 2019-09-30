using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuVisiblePart
    {
        private MenuItem[] items;
        private MenuItemPositionsMapper positionsMapper;
        private MenuItemDisplayer itemDisplayer;
        private Int32 selectedIndex;

        internal Int32 SelectedIndex
        {
            get => selectedIndex;
            set
            {
                if (value >= 0 && value < items.Length)
                {
                    Int32 oldSelected = selectedIndex;
                    selectedIndex = value;
                    items[oldSelected].Selected = false;
                    items[selectedIndex].Selected = true;
                }
            }
        }

        internal MenuVisiblePart(MenuBuilder builder, MenuItemPositionsMapper positionsMapper, MenuItemDisplayer itemDisplayer)
        {
            this.items = builder.BuildMenu();
            this.positionsMapper = positionsMapper;
            this.itemDisplayer = itemDisplayer;
            this.SelectedIndex = 0;
        }

        internal void Draw()
        {
            Point[] positions = positionsMapper.GetItemsPositions(items.Select(item => item.GetSize()).ToArray());
            foreach (var pair in items.Zip(positions, (Item, Position) => new { Item, Position }))
            {
                itemDisplayer.Draw(pair.Item, pair.Position);
            }
        }

        internal void RequestSelectedCommandExecution()
        {
            items[SelectedIndex].RequestCommandExecution();
        }
    }
}
