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
            Dictionary<Int32, List<Rectangle>> stripes = GetStripes(wallTiles);
            return GlueStripes(stripes);
        }

        private Dictionary<Int32, List<Rectangle>> GetStripes(List<Point> wallTiles)
        {
            Dictionary<Int32, List<Rectangle>> stripes = new Dictionary<Int32, List<Rectangle>>();
            Point previousTile = wallTiles[0];
            Point lastStripeStart = wallTiles[0];
            Int32 lastStripeLength = 1;

            foreach (Point wallTile in wallTiles.Skip(1))
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
            Rectangle stripe = new Rectangle
            {
                X = start.X,
                Y = start.Y,
                Width = length,
                Height = 1
            };
            Int32 key = stripe.X;
            if (stripes.ContainsKey(key))
                stripes[key].Add(stripe);
            else
                stripes.Add(key, new List<Rectangle> { stripe });
        }

        private List<Rectangle> GlueStripes(Dictionary<Int32, List<Rectangle>> stripes)
        {
            List<Rectangle> gluedRectangles = new List<Rectangle>();
            foreach (Int32 startXPosition in stripes.Keys)
            {
                IEnumerable<Rectangle> rectanglesAtCurrentX = stripes[startXPosition].Skip(1)
                    .Aggregate(seed: new { List = new List<Rectangle>(), PreviousValue = stripes[startXPosition][0] },
                               func: (acc, stripe) =>
                               {
                                   if (stripe.Y == acc.PreviousValue.Y + acc.PreviousValue.Height
                                       && acc.PreviousValue.Width == stripe.Width)
                                   {
                                       Rectangle extendedRectangle = acc.PreviousValue;
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
