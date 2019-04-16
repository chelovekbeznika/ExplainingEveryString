using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class Blueprint
    {
        public SpriteSpecification DefaultSprite { get; set; }
        public Single Height { get; set; }
        public Single Width { get; set; }
        public Single Hitpoints { get; set; }

        internal virtual IEnumerable<SpriteSpecification> GetSprites()
        {
            return new SpriteSpecification[] { DefaultSprite };
        }
        internal virtual IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return Enumerable.Empty<SpecEffectSpecification>();
        }
    }
}
