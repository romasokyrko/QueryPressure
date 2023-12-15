using QueryPressure.App;
using QueryPressure.App.Arguments;
using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.LimitCreators;
using QueryPressure.App.ProfileCreators;
using QueryPressure.App.ScriptSourceCreators;
using QueryPressure.Core.Interfaces;
using QueryPresure.Postgres.App;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var appArgs = Merge(args);
var profileFactory = new SettingsFactory<IProfile>("profile", new ICreator<IProfile>[]
        {
            new SequentialLoadCreator(),
            new SequentialWithDelayLoadCreator(),
            new LimitedConcurrencyLoadCreator(),
            new LimitedConcurrencyWithDelayLoadCreator(),
            new TargetThroughputLoadCreator()
        });

var limitFactory = new SettingsFactory<ILimit>("limit", new ICreator<ILimit>[] {
          new QueryCountLimitCreator(),
          new TimeLimitCreator()
});

var connectionProviderFactory = new SettingsFactory<IConnectionProvider>("connection", new ICreator<IConnectionProvider>[]
            {
      new PostgresConnectionProviderCreator()
});

var scriprSourceFactory = new SettingsFactory<IScriptSource>("script", new ICreator<IScriptSource>[]
            {
                new FileScriptSourceCreator()
            });

var builder = new ScenarioBuilder(profileFactory, limitFactory, connectionProviderFactory, scriprSourceFactory);
var executor = await builder.BuildAsync(appArgs, CancellationToken.None);
await executor.ExecuteAsync(CancellationToken.None);


Console.ReadLine();

ApplicationArguments Merge(string[] args)
{
    var configExtensions = new[] { ".yml", ".yaml" };
    var configFiles = args.Where(x => configExtensions.Contains(Path.GetExtension(x)));
    var scriptFile = args.Single(x => Path.GetExtension(x) == ".sql");

    var result = new ApplicationArguments();
    foreach (var configFile in configFiles)
    {
        var appArgs = Deserialize(File.ReadAllText(configFile));
        foreach (var applicationArgument in appArgs)
        {
            result.Add(applicationArgument.Key, applicationArgument.Value);
        }
    }
    result.Add("script", new ArgumentsSection()
    {
        Type = "file",
        Arguments = new()
        {
            ["path"] = scriptFile
        }
    });

    return result;
}

ApplicationArguments Deserialize(string fileContent)
{
    var deserializer = new DeserializerBuilder()
      .WithNamingConvention(CamelCaseNamingConvention.Instance)
      .Build();

    return deserializer.Deserialize<ApplicationArguments>(fileContent);
}

