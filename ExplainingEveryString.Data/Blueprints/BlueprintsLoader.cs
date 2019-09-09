using ExplainingEveryString.Data.Blueprints.AssetsExtraction.cs;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public interface IBlueprintsLoader
    {
        void Load();
        Dictionary<String, Blueprint> GetBlueprints();
    }
}
