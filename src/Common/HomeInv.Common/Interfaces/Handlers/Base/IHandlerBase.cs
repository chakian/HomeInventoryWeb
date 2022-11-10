namespace HomeInv.Common.Interfaces.Handlers
{
    public interface IHandlerBase<T, R> : IHandlerBase
    {
        R Execute(T request);
    }

    public interface IHandlerBase
    {
    }
}
