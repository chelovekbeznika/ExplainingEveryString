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
            return blueprints.SelectMany(blueprint => blueprint.GetSprites()).Where(ss => ss != null)
                .Select(ss => ss.Name).Distinct().ToList();
        }

        public static List<String> GetNecessarySounds(this IBlueprintsLoader loader)
        {
            IEnumerable<Blueprint> blueprints = loader.GetBlueprints().Values;
            return blueprints.SelectMany(blueprint => blueprint.GetSpecEffects()).Where(se => se != null && se.Sound != null)
                .Select(se => se.Sound.Name).Distinct().ToList();
        }
    }
}
