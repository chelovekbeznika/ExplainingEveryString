using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Math
{
    internal class MenuItemPositionsMapper
    {
        private Point screenSize;
        private Int32 pixelsBetweenItems;

        internal MenuItemPositionsMapper(Point screenSize, Int32 pixelsBetweenItems)
        {
            this.screenSize = screenSize;
            this.pixelsBetweenItems = pixelsBetweenItems;
        }

        internal Point[] GetItemsPositions(Point[] itemsSize)
        {
            Int32 menuHeight = itemsSize.Select(p => p.Y).Sum() + pixelsBetweenItems * (itemsSize.Length - 1);
            List<Int32> heights = itemsSize
                .Take(itemsSize.Length - 1)
                .Aggregate(
                    new List<Int32>(itemsSize.Length) { 0 },
                    (acc, size) =>
                    {
                        acc.Add(acc[acc.Count - 1] + size.Y + pixelsBetweenItems);
                        return acc;
                    });
            IEnumerable<Point> result = heights.Zip(
                itemsSize,
                (height, size) => new Point
                {
                    X = screenSize.X / 2 - size.X / 2,
                    Y = screenSize.Y / 2 - menuHeight / 2 + height
                });
            return result.ToArray();
        }
    }
}
