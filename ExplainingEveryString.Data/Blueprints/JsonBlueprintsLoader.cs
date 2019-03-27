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
        private List<Blueprint> blueprints;

        public List<Blueprint> GetBlueprints()
        {
            return blueprints;
        }

        public void Load()
        {
            blueprints = JsonDataAccessor.Instance.Load<List<Blueprint>>("blueprints.dat");
        }
    }
}
