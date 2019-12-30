using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Blueprints
{
    public interface IBlueprintsLoader
    {
        void Load();
        Dictionary<String, Blueprint> GetBlueprints();
    }
}
