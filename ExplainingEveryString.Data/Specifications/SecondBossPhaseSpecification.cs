using System;

namespace ExplainingEveryString.Data.Specifications
{
    public class SecondBossPhaseSpecification
    {
        public SpriteSpecification Sprite { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public SecondBossPowerKeepersSpecification PowerKeepersMovement { get; set; }
        public Int32 PowerKeepersAmount { get; set; }
        public Boolean UseSupport { get; set; }
    }
}
