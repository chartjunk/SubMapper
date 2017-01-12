namespace SubMapper.Metadata
{
    public interface IRevertable
    {
        void Revert();
        bool IsReverted { get; }
    }
}
