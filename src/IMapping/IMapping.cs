namespace MappingLibrary2
{
    public interface IMapping<TA, TB>
    {
        void TranslateAToB(TA A, TB B);
        void TranslateBToA(TA A, TB B);
    }
}
