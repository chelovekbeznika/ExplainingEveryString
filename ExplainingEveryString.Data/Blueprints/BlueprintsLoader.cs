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

    public static class IBlueprintsLoaderExtenstions
    {
        public static List<String> GetNeccessarySprites(this IBlueprintsLoader loader)
        {
            IEnumerable<Blueprint> blueprints = loader.GetBlueprints().Values;
            return blueprints.SelectMany(blueprint => blueprint.GetSprites())
                .Select(ss => ss.Name).Distinct().ToList();
        }

        public static List<String> GetNecessarySounds(this IBlueprintsLoader loader)
        {
            IEnumerable<Blueprint> blueprints = loader.GetBlueprints().Values;
            return blueprints.SelectMany(blueprint => blueprint.GetSpecEffects())
                .Select(se => se.Sound).Distinct().ToList();
        }
    }
}
