using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Math
{
    internal class MenuItemPositionsMapper
    {
        private Func<Point> screenSizeAccessor;
        private Int32 pixelsBetweenItems;

        internal MenuItemPositionsMapper(Func<Point> screenSizeAccessor, Int32 pixelsBetweenItems)
        {
            this.screenSizeAccessor = screenSizeAccessor;
            this.pixelsBetweenItems = pixelsBetweenItems;
        }

        internal Point[] GetItemsPositions(Point[] itemsSize)
        {
            var screenSize = screenSizeAccessor();
            var menuHeight = itemsSize.Select(p => p.Y).Sum() + pixelsBetweenItems * (itemsSize.Length - 1);
            var heights = itemsSize
                .Take(itemsSize.Length - 1)
                .Aggregate(
                    new List<Int32>(itemsSize.Length) { 0 },
                    (acc, size) =>
                    {
                        acc.Add(acc[acc.Count - 1] + size.Y + pixelsBetweenItems);
                        return acc;
                    });
            var result = heights.Zip(
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
