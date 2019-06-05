using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class PositionOnTileMap
    {
        public Int32 X { get; set; }
        public Int32 Y { get; set; }
        [DefaultValue(typeof(Vector2), "0.0, 0.0")]
        public Vector2 Offset { get; set; }
    }
}
