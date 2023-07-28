namespace ExplainingEveryString.Data.AssetsMetadata
{
    public static class AssetsMetadataAccess
    {
        private static readonly IAssetsMetadataLoader _instance = new JsonAssetsMetadataLoader();

        public static IAssetsMetadataLoader GetLoader()
        {
            return _instance;
        }
    }

    public interface IAssetsMetadataLoader
    {
        AssetsMetadata Load();
    }

    internal class JsonAssetsMetadataLoader : IAssetsMetadataLoader
    {
        private AssetsMetadata _onceCached = null;
        public AssetsMetadata Load()
        {
            _onceCached ??= JsonDataAccessor.Instance.Load<AssetsMetadata>(FileNames.AssetsMetadata);
            return _onceCached;
        }
    }
}
