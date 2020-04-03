using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Collisions
{
    internal static class SpatialPartioningHelper
    {
        private const Int32 SectorWidth = 128;

        internal static String GetSector(Vector2 point)
        {
            return Sector(Row(point.Y), Column(point.X));
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

        private static Int32 Row(Single y) => (Int32)(y / SectorWidth);

        private static String Sector(Int32 row, Int32 column) => $"{column}:{row}";
    }
}
