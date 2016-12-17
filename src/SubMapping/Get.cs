namespace SubMapper
{
    public static partial class GetMapping
    {
        public static SubMapping<TA, TB, TSubA, TSubB> SubFromHandle<TA, TB, TSubA, TSubB>(SubMappingHandle<TA, TB, TSubA, TSubB> subMappingHandle)
            where TSubA : new()
            where TSubB : new()
            => new SubMapping<TA, TB, TSubA, TSubB>();
    }
}
