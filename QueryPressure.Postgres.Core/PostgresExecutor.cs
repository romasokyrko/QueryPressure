using Npgsql;
using QueryPressure.Core.Interfaces;
using QueryPressure.Core.ScriptSources;

namespace QueryPressure.Postgres.Core
{
    internal class PostgresExecutor : IExecutable
    {
        private readonly NpgsqlConnection _connection;
        private readonly TextScript _script;

        public PostgresExecutor(IScript script, NpgsqlConnection connection)
        {
            if (script is not TextScript textScript)
                throw new ApplicationException("The only supported script type is TextScript");
            _connection = connection;
            _script = textScript;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await using var cmd = _connection.CreateCommand();
            cmd.CommandText = _script.Text;
            var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            await reader.ReadAsync(cancellationToken);
        }
    }
}