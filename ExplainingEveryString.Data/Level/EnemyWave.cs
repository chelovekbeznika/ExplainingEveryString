using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Level
{
    public class EnemyWave
    {
        public Rectangle StartRegion { get; set; }
        [DefaultValue(Int32.MaxValue)]
        public Int32 MaxEnemiesAtOnce { get; set; }
        public ActorStartInfo[] Enemies { get; set; }
    }
}
