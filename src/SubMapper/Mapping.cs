namespace SubMapper
{
    public static partial class Mapping
    {
        public static BaseMapping<TA, TB> FromTo<TA, TB>()
            => new BaseMapping<TA, TB>();
    }
}
