using QueryPressure.App.Factories;
using QueryPressure.App.Interfaces;
using QueryPressure.App.Tests;
using QueryPressure.Core.Interfaces;
using QueryPressure.Postgres.Core;
using QueryPresure.Postgres.App;
using Xunit;

namespace QueryPressure.Tests.LoadProfileTests
{
    public class ConnectionProviderFactoryTests
    {
        private readonly SettingsFactory<IConnectionProvider> _factory;

        public ConnectionProviderFactoryTests()
        {
            _factory = new SettingsFactory<IConnectionProvider>("connection", new ICreator<IConnectionProvider>[]
            {
      new PostgresConnectionProviderCreator(),
      //new MySqlConnectionProviderCreator(),
      //new RedisConnectionProviderCreator(),
      //new SqlServerConnectionProviderCreator()
            });
        }

        [Fact]
        public void Create_PostgresConnectionProvider_IsCreated()
        {
            var yml = @"
connection:
  type: postgres
  arguments:
     connectionString: Host=localhost;Database=postgres;User Id=postgres;Password=postgres;";

            var provider = TestUtils.Create(_factory, yml);
            Assert.IsType<PostgresConnectionProvider>(provider);
        }
    }
}
