using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Blueprints
{
    internal class JsonBlueprintsLoader : IBlueprintsLoader
    {
        private Dictionary<String, Blueprint> blueprints;

        public Dictionary<String, Blueprint> GetBlueprints()
        {
            return blueprints;
        }

        public void Load()
        {
            var fileName = FileNames.Blueprints;
            blueprints = JsonDataAccessor.Instance.Load<Dictionary<String, Blueprint>>(fileName);
        }
    }
}
