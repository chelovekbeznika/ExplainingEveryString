using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            String fileName = FileNames.Blueprints;
            blueprints = JsonDataAccessor.Instance.Load<Dictionary<String, Blueprint>>(fileName);
        }
    }
}
