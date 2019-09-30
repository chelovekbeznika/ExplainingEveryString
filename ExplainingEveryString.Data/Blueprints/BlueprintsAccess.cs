namespace ExplainingEveryString.Data.Blueprints
{
    public static class BlueprintsAccess
    {
        public static IBlueprintsLoader GetLoader()
        {
            return new JsonBlueprintsLoader();
        }
    }
}
