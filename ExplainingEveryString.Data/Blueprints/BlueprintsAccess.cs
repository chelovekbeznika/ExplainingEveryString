using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public static class BlueprintsAccess
    {
        public static IBlueprintsLoader GetLoader(String[] blueprintsFiles)
        {
            return new JsonBlueprintsLoader(blueprintsFiles);
        }
    }
}
