using QueryPressure.Core.Interfaces;
using System.Collections.Immutable;
using System.Diagnostics;

namespace QueryPressure.Core;
public class QueryExecutor
{
    private readonly IExecutable _executable;
    private readonly IProfile _loadProfile;
    private readonly ILimit _limit;
    private readonly ImmutableArray<IExecutionHook> _hooks;

    public QueryExecutor(IExecutable executable, IProfile loadProfile, ILimit limit)
    {
        _executable = executable;
        _loadProfile = loadProfile;
        _limit = limit;

        var hooks = ImmutableArray.CreateBuilder<IExecutionHook>();

        if (loadProfile is IExecutionHook hookProfile)
            hooks.Add(hookProfile);
        if (limit is IExecutionHook hookLimit)
            hooks.Add(hookLimit);

        _hooks = hooks.ToImmutable();
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _limit.Token);
        var token = source.Token;
        var sw = Stopwatch.StartNew();

        while (!token.IsCancellationRequested)
        {
            await _loadProfile.WhenNextCanBeExecutedAsync(token);
            var _ = _executable.ExecuteAsync(token).ContinueWith(async _ =>
            {
                await Task.WhenAll(_hooks.Select(x => x.OnQueryExecutedAsync(token)));
            }, token);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}
