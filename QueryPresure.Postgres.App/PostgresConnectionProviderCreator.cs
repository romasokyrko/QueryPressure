using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;

namespace QueryPresure.Postgres.App
{
    public class PostgresConnectionProviderCreator : ICreator<IConnectionProvider>
    {
        public string Type => "postgres";

        public IConnectionProvider Create(ArgumentsSection argumentsSection)
        {
            var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
            return new QueryPressure.Postgres.Core.PostgresConnectionProvider(connectionString);
        }
    }
}
