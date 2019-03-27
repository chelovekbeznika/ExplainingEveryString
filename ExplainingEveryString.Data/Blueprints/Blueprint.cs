using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class Blueprint
    {
        public String DefaultSpriteName { get; set; }
        public Single Height { get; set; }
        public Single Width { get; set; }
        public Single Hitpoints { get; set; }

        internal virtual IEnumerable<String> GetSprites()
        {
            return new String[] { DefaultSpriteName };
        }
    }
}
