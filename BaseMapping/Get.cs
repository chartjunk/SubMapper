namespace SubMapper
{
    public static partial class GetMapping
    {
        public static BaseMapping<TA, TB> FromTo<TA, TB>()
            => new BaseMapping<TA, TB>();
    }
}
