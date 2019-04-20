using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class GameObjectStartPosition
    {
        public Vector2 Position { get; set; }
        [DefaultValue(0)]
        public Single Angle { get; set; }
    }
}
