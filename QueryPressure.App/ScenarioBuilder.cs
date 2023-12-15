using QueryPressure.App.Arguments;
using QueryPressure.App.Factories;
using QueryPressure.Core;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.App;

public class ScenarioBuilder
{
    private readonly SettingsFactory<IProfile> _profilesFactory;
    private readonly SettingsFactory<ILimit> _limitsFactory;
    private readonly SettingsFactory<IConnectionProvider> _connectionProviderFactory;
    private readonly SettingsFactory<IScriptSource> _scriptSourceFactory;

    public ScenarioBuilder(
        SettingsFactory<IProfile> profilesFactory,
        SettingsFactory<ILimit> limitsFactory,
        SettingsFactory<IConnectionProvider> connectionProviderFactory,
        SettingsFactory<IScriptSource> scriptSourceFactory
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