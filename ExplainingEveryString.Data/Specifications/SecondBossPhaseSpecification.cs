using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class SecondBossPhaseSpecification
    {
        public SpriteSpecification Sprite { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public SecondBossPowerKeepersSpecification PowerKeepersMovement { get; set; }
        public Int32 PowerKeepersAmount { get; set; }
    }
}
