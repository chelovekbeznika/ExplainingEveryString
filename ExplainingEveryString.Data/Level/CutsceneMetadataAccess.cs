using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Data.Level
{
    public static class CutscenesMetadataAccess
    {
        public static Dictionary<String, CutsceneSpecification> LoadCutscenesMetadata()
        {
            return JsonDataAccessor.Instance.Load<Dictionary<String, CutsceneSpecification>>(FileNames.CutscenesMetadata);
        }
    }
}
