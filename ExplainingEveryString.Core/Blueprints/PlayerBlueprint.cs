using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Blueprints
{
    internal class PlayerBlueprint : Blueprint
    {
        internal Single MaxSpeed { get; set; }
        internal Single MaxAcceleration { get; set; }
        internal PlayerWeaponBlueprint Weapon { get; set; }

        internal override IEnumerable<string> GetSprites()
        {
            return new String[] { DefaultSpriteName, Weapon.BulletSpriteName };
        }
    }
}
