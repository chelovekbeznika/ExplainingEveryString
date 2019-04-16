using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.AssetsMetadata
{
    public static class AssetsMetadataAccess
    {
        public static IAssetsMetadataLoader GetLoader()
        {
            return new JsonAssetsMetadataLoader();
        }
    }

    public interface IAssetsMetadataLoader
    {
        AssetsMetadata Load();
    }

    internal class JsonAssetsMetadataLoader : IAssetsMetadataLoader
    {
        public AssetsMetadata Load()
        {
            return JsonDataAccessor.Instance.Load<AssetsMetadata>(FileNames.AssetsMetadata);
        }
    }
}
