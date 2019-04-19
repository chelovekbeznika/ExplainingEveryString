using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(1)]
        public Single Hitpoints { get; set; }

        internal virtual IEnumerable<SpriteSpecification> GetSprites()
        {
            IEnumerable<SpriteSpecification> specEffectSprites = GetSpecEffects()
                .Where(ses => ses.Sprite != null).Select(ses => ses.Sprite);
            return new SpriteSpecification[] { DefaultSprite }.Concat(specEffectSprites);
        }
        internal virtual IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return Enumerable.Empty<SpecEffectSpecification>();
        }
    }
}
