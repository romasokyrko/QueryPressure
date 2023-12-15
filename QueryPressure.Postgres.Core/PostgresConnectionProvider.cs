using Npgsql;
using QueryPressure.Core.Interfaces;

namespace QueryPressure.Postgres.Core
{
    public class PostgresConnectionProvider : IConnectionProvider
    {
        private readonly string _connectionString;

        public PostgresConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IExecutable> CreateExecutorAsync(IScriptSource scriptSource, CancellationToken cancellationToken)
        {
            var script = scriptSource.GetScriptAsync(cancellationToken);
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return new PostgresExecutor((IScript)script, connection);
        }
    }
}
