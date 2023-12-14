using QueryPressure.Core;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.Limits;
using QueryPressure.Core.LoadProfiles;


//var file = @"
//    profile:
//        type: limitedConcurrency
//        arguments:
//            limit: 10
//    limit:
//        type: queryCount
//        arguments: 
//            limit: 100";


var executor = new QueryExecutor(
    new Executable(),
    new LimitedConcurrencyWithDelayLoadProfile(2, TimeSpan.FromMilliseconds(1_000)),
    new QueryCountLimit(10));

await executor.ExecuteAsync(CancellationToken.None);

class Executable : IExecutable
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Delay(500, cancellationToken);
    }
}

//var @params = Deserialize(file);
//Console.WriteLine(@params);

//var factory = new LoadProfilesFactory(new[]
//{
//    new LimitedConcurrencyLoadProfileCreator()
//});

//var model = factory.CreateProfile(@params);

//Console.ReadLine();

//ApplicationArguments Deserialize(string fileContent)
//{
//    var deserializer = new DeserializerBuilder()
//            .WithNamingConvention(CamelCaseNamingConvention.Instance)
//            .Build();

//    return deserializer.Deserialize<ApplicationArguments>(fileContent);
//}
