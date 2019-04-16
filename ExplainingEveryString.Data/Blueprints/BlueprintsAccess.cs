using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public static class BlueprintsAccess
    {
        public static IBlueprintsLoader GetLoader()
        {
            return new JsonBlueprintsLoader();
        }
    }
}
