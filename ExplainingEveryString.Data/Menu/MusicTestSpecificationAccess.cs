namespace ExplainingEveryString.Data.Menu
{
    public static class MusicTestSpecificationAccess
    {
        public static MusicTestButtonSpecification[] Load()
        {
            return JsonDataAccessor.Instance.Load<MusicTestButtonSpecification[]>(FileNames.MusicTestMenu);
        }
    }
}
