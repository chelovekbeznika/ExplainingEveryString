using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Blueprints
{
    internal class Blueprint
    {
        internal String DefaultSpriteName { get; set; }
        internal Single Height { get; set; }
        internal Single Width { get; set; }
        internal Single Hitpoints { get; set; }

        internal virtual IEnumerable<String> GetSprites()
        {
            return new String[] { DefaultSpriteName };
        }
    }
}
