﻿using QueryPressure.App.Arguments;
using QueryPressure.App.Interfaces;
using QueryPressure.Core.Interfaces;
using QueryPressure.Postgres.Core;

namespace QueryPresure.Postgres.App
{
    public class PostgresConnectionProviderCreator : ICreator<IConnectionProvider>
    {
        public string Type => "postgres";

        public IConnectionProvider Create(ArgumentsSection argumentsSection)
        {
            var connectionString = argumentsSection.ExtractStringArgumentOrThrow("connectionString");
            return new PostgresConnectionProvider(connectionString);
        }
    }
}
