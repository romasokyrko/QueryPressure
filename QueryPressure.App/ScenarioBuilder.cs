using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class ScenarioBuilder : IScenarioBuilder
{
    private readonly ISettingsFactory<IProfile> _profilesFactory;
    private readonly ISettingsFactory<ILimit> _limitsFactory;
    private readonly ISettingsFactory<IConnectionProvider> _connectionProviderFactory;
    private readonly ISettingsFactory<IScriptSource> _scriptSourceFactory;

    public ScenarioBuilder(
        ISettingsFactory<IProfile> profilesFactory,
        ISettingsFactory<ILimit> limitsFactory,
        ISettingsFactory<IConnectionProvider> connectionProviderFactory,
        ISettingsFactory<IScriptSource> scriptSourceFactory
        )
    {
        _profilesFactory = profilesFactory;
        _limitsFactory = limitsFactory;
        _connectionProviderFactory = connectionProviderFactory;
        _scriptSourceFactory = scriptSourceFactory;
    }

    public async Task<QueryExecutor> BuildAsync(ApplicationArguments arguments, CancellationToken cancellationToken)
    {
        var profile = _profilesFactory.Create(arguments);
        var limit = _limitsFactory.Create(arguments);
        var connectionProvider = _connectionProviderFactory.Create(arguments);
        var scriptSource = _scriptSourceFactory.Create(arguments);

        var executor = await connectionProvider.CreateExecutorAsync(scriptSource, cancellationToken);
        return new QueryExecutor(executor, profile, limit);
    }
}