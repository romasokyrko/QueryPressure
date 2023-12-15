namespace QueryPressure.Core.Interfaces
{
    public interface IConnectionProvider : ISetting
    {
        Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, CancellationToken cancellationToken);
    }
}
