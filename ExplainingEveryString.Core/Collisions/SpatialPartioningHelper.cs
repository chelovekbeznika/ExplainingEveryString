using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Collisions
{
    internal static class SpatialPartioningHelper
    {
        private const Int32 SectorWidth = 128;
        private const Int32 SectorHeight = 128;

        internal static String GetSector(Vector2 point)
        {
            return Sector(Row(point.Y), Column(point.X));
        }

        internal static List<String> GetSectors(Vector2 a, Vector2 b)
        {
            Vector2 left, right;
            if (a.X < b.X)
                (left, right) = (a, b);
            else
                (left, right) = (b, a);
            var result = new List<String>();
            for (var column = Row(left.X); column <= Row(right.X); column += 1)
            {
                var leftColumnBorder = System.Math.Max(left.X, column * SectorWidth);
                var rightColumnBorder = System.Math.Min(right.X, (column + 1) * SectorWidth);
                var leftColumnBorderHeight = left.X == right.X ? left.Y
                    : left.Y + (leftColumnBorder - left.X) / (right.X - left.X) * (right.Y - left.Y);
                var rightColumnBorderHeight = left.X == right.X ? right.Y
                    : left.Y + (rightColumnBorder - left.X) / (right.X - left.X) * (right.Y - left.Y);
                var bottom = Row(leftColumnBorderHeight < rightColumnBorderHeight ? leftColumnBorderHeight : rightColumnBorderHeight);
                var top = Row(leftColumnBorderHeight > rightColumnBorderHeight ? leftColumnBorderHeight : rightColumnBorderHeight);
                for (var row = bottom; row <= top; row += 1)
                    result.Add(Sector(row, column));
            }
            return result;
        }

        internal static List<String> GetSectors(Hitbox hitbox)
        {
            var result = new List<String>();
            for (var row = Row(hitbox.Bottom); row <= Row(hitbox.Top); row += 1)
                for (var column = Column(hitbox.Left); column <= Column(hitbox.Right); column += 1)
                {
                    result.Add(Sector(row, column));
                }
            return result;
        }

        private static Int32 Column(Single x) => (Int32)(x / SectorWidth);

        private static Int32 Row(Single y) => (Int32)(y / SectorHeight);

        private static String Sector(Int32 row, Int32 column) => $"{column}:{row}";
    }
}
