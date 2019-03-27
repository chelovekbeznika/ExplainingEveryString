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
        List<Blueprint> GetBlueprints();
    }

    public static class IBlueprintsLoaderExtenstions
    {
        public static List<String> GetNeccessarySprites(this IBlueprintsLoader loader)
        {
            List<Blueprint> blueprints = loader.GetBlueprints();
            return blueprints.SelectMany(blueprint => blueprint.GetSprites()).ToList();
        }
    }
}
