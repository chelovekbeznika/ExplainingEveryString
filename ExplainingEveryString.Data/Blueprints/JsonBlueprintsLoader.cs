using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints
{
    internal class JsonBlueprintsLoader : IBlueprintsLoader
    {
        private Dictionary<String, Blueprint> blueprints;
        private String[] filenames = new String[] { "blueprints" };

        public JsonBlueprintsLoader(String[] blueprintsFiles)
        {
            this.filenames = blueprintsFiles;
        }

        public Dictionary<String, Blueprint> GetBlueprints()
        {
            return blueprints;
        }

        public void Load()
        {
            blueprints = filenames
                .Select(filename => FileNames.GetJsonBlueprintsPath(filename))
                .SelectMany(filename => JsonDataAccessor.Instance.Load<Dictionary<String, Blueprint>>(filename))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}
