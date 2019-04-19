using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class BulletSpecification
    {
        public Single Speed { get; set; }
        public Single Damage { get; set; }
        public Single Range { get; set; }

        public SpriteSpecification Sprite { get; set; }
    }
}
