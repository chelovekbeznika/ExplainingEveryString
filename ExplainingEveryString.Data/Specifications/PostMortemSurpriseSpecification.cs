using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class PostMortemSurpriseSpecification
    {
        [DefaultValue(null)]
        public PostMortemWeaponSpecification Weapon { get; set; }
        [DefaultValue(null)]
        public PostMortemSpawnSpecificaton Spawn { get; set; }
    }

    public class PostMortemWeaponSpecification
    {
        public BarrelSpecification[] Barrels { get; set; }
        public AimType AimType { get; set; }
    }

    public class PostMortemSpawnSpecificaton
    {
        public Int32 AvengersAmount { get; set; }
        public String AvengersType { get; set; }
        public SpawnPositionSelectorSpecification PositionSelector { get; set; }
    }
}
