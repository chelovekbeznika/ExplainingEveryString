using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Tiles
{
    internal class WallsOptimizer
    {
        internal List<Rectangle> GetWalls(List<Point> wallTiles)
        {
            if (wallTiles.Count == 0)
                return new List<Rectangle>();
            var stripes = GetStripes(wallTiles);
            return GlueStripes(stripes);
        }

        private Dictionary<Int32, List<Rectangle>> GetStripes(List<Point> wallTiles)
        {
            var stripes = new Dictionary<Int32, List<Rectangle>>();
            var previousTile = wallTiles[0];
            var lastStripeStart = wallTiles[0];
            var lastStripeLength = 1;

            foreach (var wallTile in wallTiles.Skip(1))
            {
                if (previousTile.X == wallTile.X - 1 && previousTile.Y == wallTile.Y)
                {
                    lastStripeLength += 1;
                }
                else
                {
                    AddStripe(stripes, lastStripeStart, lastStripeLength);
                    lastStripeStart = wallTile;
                    lastStripeLength = 1;
                }
                previousTile = wallTile;
            }
            AddStripe(stripes, lastStripeStart, lastStripeLength);

            return stripes;
        }

        private void AddStripe(Dictionary<Int32, List<Rectangle>> stripes, Point start, Int32 length)
        {
            var stripe = new Rectangle
            {
                X = start.X,
                Y = start.Y,
                Width = length,
                Height = 1
            };
            var key = stripe.X;
            if (stripes.ContainsKey(key))
                stripes[key].Add(stripe);
            else
                stripes.Add(key, new List<Rectangle> { stripe });
        }

        private List<Rectangle> GlueStripes(Dictionary<Int32, List<Rectangle>> stripes)
        {
            var gluedRectangles = new List<Rectangle>();
            foreach (var startXPosition in stripes.Keys)
            {
                var rectanglesAtCurrentX = stripes[startXPosition].Skip(1)
                    .Aggregate(seed: new { List = new List<Rectangle>(), PreviousValue = stripes[startXPosition][0] },
                               func: (acc, stripe) =>
                               {
                                   if (stripe.Y == acc.PreviousValue.Y + acc.PreviousValue.Height
                                       && acc.PreviousValue.Width == stripe.Width)
                                   {
                                       var extendedRectangle = acc.PreviousValue;
                                       extendedRectangle.Height += 1;
                                       return new { acc.List, PreviousValue = extendedRectangle };
                                   }
                                   else
                                   {
                                       acc.List.Add(acc.PreviousValue);
                                       return new { acc.List, PreviousValue = stripe };
                                   }
                               },
                               resultSelector: (acc) => acc.List.Concat( new Rectangle[] { acc.PreviousValue } ));

                gluedRectangles.AddRange(rectanglesAtCurrentX);
            }
            return gluedRectangles;
        }
    }
}
